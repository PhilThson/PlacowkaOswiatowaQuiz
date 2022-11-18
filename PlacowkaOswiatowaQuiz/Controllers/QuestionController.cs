using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class QuestionController : Controller
    {
        private readonly QuizApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public QuestionController(QuizApiSettings apiSettings,
            IHttpClientFactory httpClientFactory)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var questions = new List<QuestionViewModel>();

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);

            var response = await httpClient.GetAsync(_apiSettings.Questions);

            response.EnsureSuccessStatusCode();

            questions = await response.Content
                .ReadFromJsonAsync<List<QuestionViewModel>>();

            return View(questions);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var question = new QuestionViewModel();

            if (id == default(int))
                return View(question);

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);

            var response = await httpClient.GetAsync(
                $"{_apiSettings.Questions}/{id}");

            response.EnsureSuccessStatusCode();

            question = await response.Content
                .ReadFromJsonAsync<QuestionViewModel>();

            //return PartialView("Edit", question);
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(QuestionViewModel questionVM)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            //var json = JsonConvert.SerializeObject(questionVM);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);
            var response = new HttpResponseMessage();

            if (!questionVM.Id.HasValue)
                response = await httpClient.PostAsJsonAsync(_apiSettings.Questions,
                    questionVM);
            else
                response = await httpClient.PutAsJsonAsync(_apiSettings.Questions,
                    questionVM);

            if (!response.IsSuccessStatusCode)
            {
                //Dodanie informacji (ViewBag) że operacja się nie powiodła
                TempData["errorAlert"] = "Nieudana próba edycji pytania";
                return View(questionVM);
            }

            TempData["successAlert"] = "Poprawnie zaktualizowano/dodano pytanie";
            return RedirectToAction(nameof(Index));
        }
    }
}

