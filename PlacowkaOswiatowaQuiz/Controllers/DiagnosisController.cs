using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class DiagnosisController : Controller
    {
        private readonly IQuestionsSetService _questionsSetService;
        private readonly IDiagnosisService _diagnosisService;

        public DiagnosisController(IQuestionsSetService questionsSetService,
            IDiagnosisService diagnosisService)
        {
            _questionsSetService = questionsSetService;
            _diagnosisService = diagnosisService;
        }

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
                TempData["saveSuccess"] = "Poprawnie zapisno ocenę.";
                return PartialView("_Result", resultVM);
            }
            catch (HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nie udało się zapisać oceny" +
                    $"\nOdpowiedź serwera: '{e.Message}'";
                return PartialView("_Result", resultVM);
            }
        }
        #endregion

        #region Wyświetlanie formularza diagnozy
        public async Task<IActionResult> Diagnosis([FromQuery] int diagnosisId)
        {
            //Można założyć, że zawsze wszystkie zestawy pytań mają zostać zadane
            var diagnosis = new DiagnosisViewModel();
            var questionsSets = new List<QuestionsSetViewModel>();
            try
            {
                diagnosis = await _diagnosisService.GetDiagnosisById(diagnosisId);
                questionsSets = await _questionsSetService.GetAllQuestionsSets();
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
                //Po utworzeniu diagnozy, przekierowanie do formualrza (1 zestaw pytań)
                return RedirectToAction(nameof(Diagnosis), new { diagnosisId = createdDiagnosis.Id});
            }
            catch (HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nie udało się utworzyć formularza diagnozy." +
                    $"\nOdpowiedź serwera: '{e.Message}'";
                return View(diagnosisVM);
            }
        }
        #endregion

        #region Podsumowanie/szczegóły formularza diagnozy
        public async Task<IActionResult> Details([FromRoute] int diagnosisId)
        {
            var diagnosis = new DiagnosisViewModel();
            try
            {
                diagnosis = await _diagnosisService.GetDiagnosisById(diagnosisId);
                return View(diagnosis);
            }
            catch(HttpRequestException e)
            {
                TempData["errorAlert"] = "Nie udało się utworzyć formularza diagnozy o " +
                    $"podanym identyfikatorze {diagnosisId}. Odpowiedź serwera: '{e.Message}'";
                return View(diagnosis);
            }
        }
        #endregion
    }
}

