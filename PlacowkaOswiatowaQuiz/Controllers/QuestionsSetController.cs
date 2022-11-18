using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class QuestionsSetController : Controller
    {
        private readonly QuizApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public QuestionsSetController(QuizApiSettings apiSettings,
            IHttpClientFactory httpClientFactory)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
        }

        #region QuestionsSet
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var questionsSets = new List<QuestionsSetViewModel>();

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);

            //var uri = new Uri($"{_apiSettings.QuestionsSets}");

            var response = await httpClient.GetAsync(_apiSettings.QuestionsSets);

            response.EnsureSuccessStatusCode();

            questionsSets = await response.Content
                .ReadFromJsonAsync<List<QuestionsSetViewModel>>();

            return View(questionsSets);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var questionsSet = new QuestionsSetViewModel();

            if (id == default(int))
                return View(questionsSet);

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);

            var response = await httpClient.GetAsync(
                $"{_apiSettings.QuestionsSets}/{id}");

            response.EnsureSuccessStatusCode();

            questionsSet = await response.Content
                .ReadFromJsonAsync<QuestionsSetViewModel>();

            return View(questionsSet);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(QuestionsSetViewModel questionsSetVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //var json = JsonConvert.SerializeObject(questionVM);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);
            var response = await httpClient.PostAsJsonAsync(_apiSettings.QuestionsSets,
                questionsSetVM);
            //response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode)
            {
                //Dodanie informacji (ViewBag) że operacja się nie powiodła
                return View(questionsSetVM);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Ratings
        [HttpPost]
        public async Task<IActionResult> EditRating(RatingViewModel ratingVM)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);
            var response = await httpClient.PutAsJsonAsync(_apiSettings.Ratings,
                ratingVM);

            response.EnsureSuccessStatusCode();

            return RedirectToAction(nameof(Details), new { id = ratingVM.QuestionsSetId });
            //ew. renderować tylko partial view, tylko trzeba pobrać kolekcję
            //ocen do zestawu pytań
            //return PartialView("_Ratings", )
        }
        #endregion

        #region Skill
        [HttpPost]
        public async Task<IActionResult> EditSkill(int id, string skill)
        {
            if (!ModelState.IsValid || skill.Length == 0)
                return RedirectToAction(nameof(Details), new {id = id});

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);
            var response = await httpClient.PatchAsync(
                $"{_apiSettings.QuestionsSets}/{id}?skill={skill}", null);
            var updated = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                TempData["errorAlert"] = "Nieudana próba edycji umiejętności";
            else if (updated == skill)
                TempData["infoAlert"] = "Nie dokonano zmian";
            else
                TempData["successAlert"] = "Poprawnie zaktualizowano umiejętności";

            return RedirectToAction(nameof(Details), new { id = id });
        }
        #endregion

        #region QuestionsSet Area
        [HttpPost]
        public async Task<IActionResult> EditArea(int id, byte areaId)
        {
            if (!ModelState.IsValid || areaId == default(byte))
                return RedirectToAction(nameof(Details), new { id = id });

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);
            var response = await httpClient.PatchAsync(
                $"{_apiSettings.QuestionsSets}/{id}?skill={skill}", null);
            var updated = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                TempData["errorAlert"] = "Nieudana próba edycji obszaru zestawu pytań";
            //if (updated == skill)
            //    TempData["infoAlert"] = "Nie dokonano zmian";
            else
                TempData["successAlert"] = "Poprawnie zaktualizowano obszar zestawu pytań";

            return RedirectToAction(nameof(Details), new { id = id });
        }
        #endregion
    }
}

