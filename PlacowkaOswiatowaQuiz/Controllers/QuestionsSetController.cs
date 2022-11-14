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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class QuestionsSetController : Controller
    {
        private readonly QuizApiSettings _apiSettings;

        public QuestionsSetController(QuizApiSettings apiSettings)
        {
            _apiSettings = apiSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var questionsSets = new List<QuestionsSetViewModel>();
            using (var client = new HttpClient())
            {
                var uri = new Uri(
                    $"{_apiSettings.Host}" + "/" +
                    $"{_apiSettings.MainController}" + "/" +
                    $"{_apiSettings.QuestionsSets}");

                var response = await client.GetAsync(uri);

                questionsSets = await response.Content
                    .ReadFromJsonAsync<List<QuestionsSetViewModel>>();
            }

            return View(questionsSets);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var questionsSet = new QuestionsSetViewModel();

            if (id == default(int))
                return View(questionsSet);

            using (var client = new HttpClient())
            {
                var uri = new Uri(QueryHelpers.AddQueryString(
                    $"{_apiSettings.Host}" + "/" +
                    $"{_apiSettings.MainController}" + "/" +
                    $"{_apiSettings.QuestionsSets}", "id", $"{id}"));

                var response = await client.GetAsync(uri);

                questionsSet = await response.Content
                    .ReadFromJsonAsync<QuestionsSetViewModel>();
            }

            return View(questionsSet);
        }

        //[HttpPost]
        //public async Task<IActionResult> Edit(QuestionViewModel questionVM)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    var json = JsonConvert.SerializeObject(questionVM);
        //    var data = new StringContent(json, Encoding.UTF8, "application/json");

        //    using (var client = new HttpClient())
        //    {
        //        var uri = new Uri(
        //            $"{_apiSettings.Host}" + "/" +
        //            $"{_apiSettings.MainController}" + "/" +
        //            $"{_apiSettings.QuestionsSets}");

        //        var response = await client.PostAsJsonAsync(uri, questionVM);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            //Dodanie informacji (ViewBag) że operacja się nie powiodła
        //            return View(questionVM);
        //        }
        //    }

        //    return RedirectToAction(nameof(Index));
        //}
    }
}

