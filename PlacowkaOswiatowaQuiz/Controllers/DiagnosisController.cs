﻿using System;
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
                diagnosis = await Map(diagnosisId);
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
                return RedirectToAction(nameof(DiagnosisForm), new { diagnosisId = createdDiagnosis.Id});
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
        [HttpGet("Diagnosis/Details/{diagnosisId}")]
        public async Task<IActionResult> Details([FromRoute] int diagnosisId)
        {
            var diagnosisSummary = new DiagnosisSummaryViewModel();
            try
            {
                diagnosisSummary = await Map(diagnosisId);
                return View(diagnosisSummary);
            }
            catch(HttpRequestException e)
            {
                TempData["errorAlert"] = "Nie udało się pobrać formularza diagnozy o " +
                    $"podanym identyfikatorze {diagnosisId}. Odpowiedź serwera: '{e.Message}'";
                return View(diagnosisSummary);
            }
        }
        #endregion

        #region Metody prywatne
        private async Task<DiagnosisSummaryViewModel> Map(int diagnosisId)
        {
            var diagnosis = await _diagnosisService.GetDiagnosisById(diagnosisId);

            var questionsSets =
                await _questionsSetService.GetAllQuestionsSets(diagnosis.Difficulty.Id);

            return new DiagnosisSummaryViewModel
            {
                Id = diagnosis.Id,
                SchoolYear = diagnosis.SchoolYear,
                Student = diagnosis.Student,
                Employee = diagnosis.Employee,
                Difficulty = diagnosis.Difficulty,
                Results = diagnosis.Results,
                QuestionsSetsIds = questionsSets.Select(qs => qs.Id).ToList(),
                QuestionsSets = questionsSets
            };
        }
        #endregion
    }
}

