using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class DiagnosisController : Controller
    {
        #region Prywatne pola
        private readonly IQuestionsSetService _questionsSetService;
        private readonly IDiagnosisService _diagnosisService;
        #endregion

        #region Konstruktor
        public DiagnosisController(IQuestionsSetService questionsSetService,
            IDiagnosisService diagnosisService)
        {
            _questionsSetService = questionsSetService;
            _diagnosisService = diagnosisService;
        }
        #endregion

        #region Lista wszystkich utworzonych formularzy
        public async Task<IActionResult> Index()
        {
            var diagnosis = new List<DiagnosisViewModel>();
            try
            {
                diagnosis = await _diagnosisService.GetAllDiagnosis();
                return View(diagnosis);
            }
            catch (HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nie udało się pobrać wszystkich" +
                    $"formularzy diagnóz. \nOdpowiedź serwera: '{e.Message}'";
                return View(diagnosis);
            }
        }
        #endregion

        #region Zapis wyniku wybranej oceny zestawu pytań
        [HttpPost]
        public async Task<PartialViewResult> SaveResult(ResultViewModel resultVM)
        {
            if (ModelState.ContainsKey("QuestionsSetRating.RatingDescription"))
            {
                //Manualne usunięcie walidacji niewykorzystywanej właściwości ViewModel'u
                ModelState["QuestionsSetRating.RatingDescription"].Errors.Clear();
                ModelState["QuestionsSetRating.RatingDescription"].ValidationState =
                    ModelValidationState.Valid;
            }

            if (!ModelState.IsValid)
                return PartialView("_Result", resultVM);

            try
            {
                await _diagnosisService.CreateResult(resultVM);
                ViewBag.SaveSuccess = "Poprawnie zapisno ocenę.";
                return PartialView("_Result", resultVM);
            }
            catch (Exception e)
            {
                TempData["errorAlert"] = $"Nie udało się zapisać oceny" +
                    $"\nOdpowiedź serwera: '{e.Message}'";
                return PartialView("_Result", resultVM);
            }
        }
        #endregion

        #region Wyświetlanie formularza diagnozy
        public async Task<IActionResult> DiagnosisForm([FromQuery] int diagnosisId)
        {
            var diagnosis = new DiagnosisViewModel();
            try
            {
                diagnosis = await _diagnosisService.GetDiagnosisById(diagnosisId);

                var questionsSets =
                    await _questionsSetService.GetAllQuestionsSets(diagnosis.Difficulty.Id);

                //Lista identyfikatorów zestawów pytań, których modele będą pobrane 
                //asynchronicznie na etapie przełączania zestawów na formularzu diagnozy
                diagnosis.QuestionsSetsIds = questionsSets.Select(qs => qs.Id).ToList();
                return View(diagnosis);
            }
            catch (HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nie udało się pobrać formularza diagnozy" +
                    $"\nOdpowiedź serwera: '{e.Message}'";
                return View(diagnosis);
            }
        }

        public async Task<IActionResult> QuestionsSetPartial([FromQuery] int questionsSetId)
        {
            try
            {
                var questionsSet = await _questionsSetService.GetQuestionsSetById(questionsSetId);
                return PartialView("_QuestionsSet", questionsSet);
            }
            catch (HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nie udało się pobrać zestawu pytań." +
                    $"\nOdpowiedź serwera: '{e.Message}'";
                return PartialView(null);
            }
        }

        public async Task<IActionResult> ResultPartial([FromQuery] int diagnosisId,
            [FromQuery] int questionsSetId)
        {
            var result = await _diagnosisService.GetResultByDiagnosisQuestionsSetIds(
                    diagnosisId, questionsSetId);

            return PartialView("_Result", result);
        }
        #endregion

        #region Tworzenie formularza diagnozy
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDiagnosisViewModel diagnosisVM)
        {
            if (!ModelState.IsValid)
                return View(diagnosisVM);
            try
            {
                var createdDiagnosis = await _diagnosisService.CreateDiagnosis(diagnosisVM);
                //Po utworzeniu diagnozy, przekierowanie do formularza (pierwszego zestawu pytań)
                return RedirectToAction(nameof(DiagnosisForm), new { diagnosisId = createdDiagnosis.Id });
            }
            catch (Exception e)
            {
                TempData["errorAlert"] = $"Nie udało się utworzyć formularza diagnozy." +
                    $"\nOdpowiedź serwera: '{e.Message}'";
                return View(diagnosisVM);
            }
        }
        #endregion

        #region Podsumowanie/szczegóły formularza diagnozy
        //Przekazywanie parametru przez Route wymaga ręcznego zadeklarowania endpointu
        [HttpGet("Diagnosis/Details/{diagnosisId}")]
        public async Task<IActionResult> Details([FromRoute] int diagnosisId)
        {
            var diagnosisSummary = new DiagnosisSummaryViewModel();
            try
            {
                diagnosisSummary = await GetDiagnosisSummary(diagnosisId);
                return View(diagnosisSummary);
            }
            catch(HttpRequestException e)
            {
                TempData["errorAlert"] = "Nie udało się pobrać formularza diagnozy o " +
                    $"podanym identyfikatorze {diagnosisId}. Odpowiedź serwera: '{e.Message}'";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region Metody prywatne
        private async Task<DiagnosisSummaryViewModel> GetDiagnosisSummary(int diagnosisId)
        {
            var diagnosis = await _diagnosisService.GetDiagnosisById(diagnosisId);

            var askedQuestionSetsIds = new List<int>();
            //QuestionsSetRating (ocena zestawu pytań) jest wymagana do podania,
            //więc na pewno istnieje, jeżeli istnieje powiązany z diagnozą Result (wynik)
            if (diagnosis.Results?.Count > 0)
                askedQuestionSetsIds = diagnosis.Results
                    .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();

            var questionsSets =
                await _questionsSetService.GetQuestionsSetsByIds(askedQuestionSetsIds) ??
                new List<QuestionsSetViewModel>();

            return new DiagnosisSummaryViewModel
            {
                Id = diagnosis.Id,
                Institution = diagnosis.Institution,
                SchoolYear = diagnosis.SchoolYear,
                CounselingCenter = diagnosis.CounselingCenter,
                Student = diagnosis.Student,
                Employee = diagnosis.Employee,
                Difficulty = diagnosis.Difficulty,
                ReportId = diagnosis.ReportId,
                Results = diagnosis.Results,
                CreatedDate = diagnosis.CreatedDate,
                QuestionsSets = questionsSets
            };
        }
        #endregion
    }
}

