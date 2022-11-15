using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class StudentController : Controller
    {
        private readonly QuizApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public StudentController(QuizApiSettings apiSettings,
            IHttpClientFactory httpClientFactory)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var students = new List<StudentViewModel>();

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);

            var uri = new Uri(_apiSettings.Students);

            var response = await httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            students = await response.Content
                .ReadFromJsonAsync<List<StudentViewModel>>();

            return View(students);
        }
    }
}

