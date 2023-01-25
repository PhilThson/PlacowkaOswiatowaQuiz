using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Services;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class ReportController : Controller
    {
        #region Pola prywatne
        private readonly IDiagnosisService _diagnosisService;
        private readonly IQuestionsSetService _questionsSetService;
        #endregion

        #region Konstruktor
        public ReportController(IDiagnosisService diagnosisService,
            IQuestionsSetService questionsSetService)
        {
            _diagnosisService = diagnosisService;
            _questionsSetService = questionsSetService;
        }
        #endregion

        #region Akcje

        #region Wyświetlanie regionu przycisków
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
            var model = new DiagnosisReportKeyViewModel
            {
                DiagnosisId = diagnosisId
            };
            try
            {
                var baseReport = await _diagnosisService.CreateDiagnosisReport(diagnosisId);
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
                return BadRequest("Wystąpił błąd połączenia do serwera.");
            }
        }
        #endregion

        #region Pobranie raportu po identyfikatorze
        public async Task<IActionResult> GetDiagnosisReport([FromQuery] int reportId)
        {
            try
            {
                var reportDto = await _diagnosisService.GetDiagnosisReportById(reportId);
                if (reportDto == null)
                    return NotFound();

                using var pdfStream = new MemoryStream();
                pdfStream.Write(reportDto.Content, 0, reportDto.Content.Length);
                pdfStream.Position = 0;

                //var pdfArray = pdfStream.ToArray();
                //var base64stream = Convert.ToBase64String(pdfArray);
                //return new FileStreamResult(pdfStream, "application/pdf");
                //return File(Convert.ToBase64String(pdfArray), "application/pdf;base64");
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
                var reportDto = await _diagnosisService.GetDiagnosisReportById(reportId);
                if (reportDto == null)
                    return NotFound();

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
            var diagnosis = await _diagnosisService.GetDiagnosisById(diagnosisId);

            var askedQuestionSetsIds = new List<int>();

            if (diagnosis.Results?.Count > 0)
                askedQuestionSetsIds = diagnosis.Results
                    .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();
            var questionsSets =
                await _questionsSetService.GetQuestionsSetsByIds(askedQuestionSetsIds) ??
                new List<QuestionsSetViewModel>();

            var masteredQSIds = diagnosis.Results.Where(d => d.RatingLevel > 4)
                .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();
            var toImproveQSIds = diagnosis.Results.Where(d => d.RatingLevel < 5)
                .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();

            return new DiagnosisToPdfViewModel
            {
                Id = diagnosis.Id,
                Institution = diagnosis.Institution,
                SchoolYear = diagnosis.SchoolYear,
                CounselingCenter = diagnosis.CounselingCenter,
                Student = diagnosis.Student,
                Employee = diagnosis.Employee,
                CreatedDate = diagnosis.CreatedDate,
                Difficulty = diagnosis.Difficulty,
                Results = diagnosis.Results,
                QuestionsSetsMastered =
                    questionsSets.Where(qs => masteredQSIds.Contains(qs.Id))
                    .OrderBy(qs => qs.Area.Name).ToList(),
                QuestionsSetsToImprove =
                    questionsSets.Where(qs => toImproveQSIds.Contains(qs.Id))
                    .OrderBy(qs => qs.Area.Name).ToList(),
            };
        }
        #endregion

        #endregion
    }
}
