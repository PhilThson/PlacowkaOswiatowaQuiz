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

        public QuestionController(QuizApiSettings apiSettings)
        {
            _apiSettings = apiSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var questions = new List<QuestionViewModel>();
            using (var client = new HttpClient())
            {
                var uri = new Uri(
                    $"{_apiSettings.Host}" + "/" +
                    $"{_apiSettings.MainController}" + "/" +
                    $"{_apiSettings.Questions}");

                var response = await client.GetAsync(uri);

                questions = await response.Content
                    .ReadFromJsonAsync<List<QuestionViewModel>>();
            }

            return View(questions);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var question = new QuestionViewModel();

            if (id == default(int))
                return View(question);

            using (var client = new HttpClient())
            {
                var uri = new Uri(QueryHelpers.AddQueryString(
                    $"{_apiSettings.Host}" + "/" +
                    $"{_apiSettings.MainController}" + "/" +
                    $"{_apiSettings.Question}", "id", $"{id}"));

                var response = await client.GetAsync(uri);

                question = await response.Content
                    .ReadFromJsonAsync<QuestionViewModel>();
            }

            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(QuestionViewModel questionVM)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var json = JsonConvert.SerializeObject(questionVM);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var uri = new Uri(
                    $"{_apiSettings.Host}" + "/" +
                    $"{_apiSettings.MainController}" + "/" +
                    $"{_apiSettings.Question}");

                var response = await client.PostAsJsonAsync(uri, questionVM);

                if (!response.IsSuccessStatusCode)
                {
                    //Dodanie informacji (ViewBag) że operacja się nie powiodła
                    return View(questionVM);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

