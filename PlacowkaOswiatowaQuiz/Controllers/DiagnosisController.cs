using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> GetNextQuestionsSet(int id)
        {
            //Pobranie pełnego zestawu pytań i wyświetlenie PartialView
            var questionsSet = await _questionsSetService.GetQuestionsSetById(id);

            return View(questionsSet);
            //return PartialView()
        }

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
                await _diagnosisService.CreateDiagnosis(diagnosisVM);
                //Tutaj przekierowanie odrazu do formualrza (1 zestaw pytań)
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nie udało się utworzyć formularza diagnozy." +
                    $"\nOdpowiedź serwera: '{e.Message}'";
                return View(diagnosisVM);
            }
        }
    }
}

