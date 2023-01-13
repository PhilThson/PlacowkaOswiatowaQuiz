using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class ReportController : Controller
    {
        #region Pola prywatne
        private readonly IDiagnosisService _diagnosisService;
        #endregion

        #region Konstruktor
        public ReportController(IDiagnosisService diagnosisService)
        {
            _diagnosisService = diagnosisService;
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

            return PartialView("GenerateReport", model);
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
                return PartialView("GenerateReport", model);
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
                //Pytanie czy ten komunikat pojawi się na dole podsumowania
                //czy trzeba zwracać PartialView
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

        #endregion
    }
}
