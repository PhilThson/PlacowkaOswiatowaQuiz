using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class UczenController : Controller
    {
        private readonly QuizApiSettings _apiSettings;

        public UczenController(QuizApiSettings apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public async Task<IActionResult> WszyscyUczniowie()
        {
            var students = new List<UczenViewModel>();

            using (var client = new HttpClient())
            {
                var uri = new Uri(
                    $"{_apiSettings.Host}" + "/" +
                    $"{_apiSettings.MainController}" + "/" +
                    $"{_apiSettings.Students}");

                var response = await client.GetAsync(uri);

                students = await response.Content
                    .ReadFromJsonAsync<List<UczenViewModel>>();
            }

            return View(students);
        }
    }
}

