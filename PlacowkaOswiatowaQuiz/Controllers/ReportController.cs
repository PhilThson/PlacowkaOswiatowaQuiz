﻿using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Helpers;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Services;
using PlacowkaOswiatowaQuiz.Shared.DTOs;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class ReportController : Controller
    {
        #region Pola prywatne
        private readonly IHttpClientService _httpClient;
        private readonly IReportService _reportService;
        #endregion

        #region Konstruktor
        public ReportController(IHttpClientService httpClient,
            IReportService reportService)
        {
            _httpClient = httpClient;
            _reportService = reportService;
        }
        #endregion

        #region Akcje

        #region Wyświetlanie regionu przycisków generowania raportu
        public IActionResult DiagnosisReportPartial([FromQuery] int diagnosisId, 
            [FromQuery] int reportId)
        {
            var model = new DiagnosisReportKeyViewModel
            {
                DiagnosisId = diagnosisId,
                ReportId = reportId == default ? null : reportId
            };

            return PartialView("_GenerateReport", model);
        }
        #endregion

        #region Generowanie raportu
        [HttpPost("Report/Generate/{diagnosisId}")]
        public async Task<IActionResult> GenerateDiagnosisReport([FromRoute] int diagnosisId)
        {
            try
            {
                if (diagnosisId == default)
                    throw new DataNotFoundException(
                        "Nie znaleziono diagnozy o podanym identyfikatorze");

                var model = new DiagnosisReportKeyViewModel
                {
                    DiagnosisId = diagnosisId
                };

                await Task.Delay(2000);

                var baseReport = await _reportService.CreateDiagnosisReport(diagnosisId);
                model.ReportId = baseReport.Id;
                //ViewBag.SaveSuccess = "Poprawnie wygenerowano raport";
                return PartialView("_GenerateReport", model);
            }
            catch (HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nie udało się utworzyć raportu diagnozy o " +
                    $"identyfikatorze {diagnosisId}. Odpowiedź serwera: " +
                    $"'{e.Message}'";
                return BadRequest("Wystąpił błąd połączenia do serwera");
            }
            catch(Exception e)
            {
                TempData["errorAlert"] = e.Message;
                return BadRequest($"Wystąpił błąd. '{e.Message}");
            }
        }
        #endregion

        #region Pobranie raportu po identyfikatorze
        public async Task<IActionResult> GetDiagnosisReport([FromQuery] int reportId)
        {
            try
            {
                if (reportId == default)
                    return NotFound();

                var reportDto = await _httpClient.GetItemById<ReportDto>(reportId);
                if (reportDto == null)
                    return NotFound();

                using var pdfStream = new MemoryStream();
                pdfStream.Write(reportDto.Content, 0, reportDto.Content.Length);
                pdfStream.Position = 0;

                return File(
                    reportDto.Content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    reportDto.Name);
            }
            catch (HttpRequestException e)
            {
                //ten komunikat się nie wyświetli dopóki nie zostanie przeładowany partialView
                TempData["errorAlert"] = $"Nie udało się pobrać raportu o " +
                    $"identyfikatorze {reportId}. Odpowiedź serwera: " +
                    $"'{e.Message}'";
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region Wyświetlenie raportu po identyfikatorze
        [HttpGet("Report/ShowDiagnosisReport/{reportId}")]
        public async Task<IActionResult> ShowDiagnosisReport([FromRoute] int reportId)
        {
            try
            {
                if (reportId == default)
                    return NotFound();

                var reportDto = await _httpClient.GetItemById<ReportDto>(reportId);
                //nie jest sorawdzane czy reportDto jest nullem, bo jeżeli API nie
                //znajdzie, to zwróci NotFound co zostanie wychwycone w sekcji catch

                var pdfStream = new MemoryStream();
                pdfStream.Write(reportDto.Content, 0, reportDto.Content.Length);
                pdfStream.Position = 0;

                ViewData["Title"] = "Podgląd diagnozy";
                //Natychmiastowe pobieranie
                //return new FileStreamResult(pdfStream, "application/download");
                //Wyświetlenie na stronie
                return new FileStreamResult(pdfStream, "application/pdf");
            }
            catch (HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nie udało się pobrać raportu o " +
                    $"identyfikatorze {reportId}. Odpowiedź serwera: " +
                    $"'{e.Message}'";
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region Podgląd raportu
        
        public async Task<IActionResult> Preview(int diagnosisId)
        {
            await Task.Delay(2000);
            var diagnosisToPdf = await GetDiagnosisPdf(diagnosisId);

            return PartialView("_Preview", diagnosisToPdf);
        }

        //Ta sama mechanika jest wykorzystywana po stronie API do generowania raportu
        private async Task<DiagnosisToPdfViewModel> GetDiagnosisPdf(int diagnosisId)
        {
            var diagnosis = await _httpClient.GetItemById<DiagnosisViewModel>(diagnosisId);

            var askedQuestionSetsIds = new List<int>();

            if (diagnosis.Results?.Count > 0)
                askedQuestionSetsIds = diagnosis.Results
                    .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();

            var questionsSets =
                await _httpClient.GetAllItems<QuestionsSetViewModel>(
                    ("askedQuestionSetsIds", string.Join(',', askedQuestionSetsIds))) ??
                    new List<QuestionsSetViewModel>();

            var masteredQSIds = diagnosis.Results.Where(d => d.RatingLevel > 4)
                .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();
            var toImproveQSIds = diagnosis.Results.Where(d => d.RatingLevel < 5)
                .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();

            var diagnosisToPdf = (DiagnosisToPdfViewModel)diagnosis;

            diagnosisToPdf.QuestionsSetsMastered =
                    questionsSets.Where(qs => masteredQSIds.Contains(qs.Id))
                    .OrderBy(qs => qs.Area.Name).ToList();
            diagnosisToPdf.QuestionsSetsToImprove =
                    questionsSets.Where(qs => toImproveQSIds.Contains(qs.Id))
                    .OrderBy(qs => qs.Area.Name).ToList();

            return diagnosisToPdf;
        }
        #endregion

        #endregion
    }
}
