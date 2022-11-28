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
        private readonly QuizApiUrl _apiUrl;
        private readonly QuizApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public StudentController(QuizApiSettings apiSettings,
            IHttpClientFactory httpClientFactory,
            QuizApiUrl apiUrl)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
            _apiUrl = apiUrl;
        }

        public async Task<IActionResult> Index()
        {
            var students = new List<StudentViewModel>();
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(_apiSettings.Students);
            response.EnsureSuccessStatusCode();
            students = await response.Content
                .ReadFromJsonAsync<List<StudentViewModel>>();

            return View(students);
        }

        public async Task<IActionResult> GetAllStudents()
        {
            var students = new List<StudentViewModel>();
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(_apiSettings.Students);
            response.EnsureSuccessStatusCode();
            students = await response.Content
                .ReadFromJsonAsync<List<StudentViewModel>>();

            return Ok(students);
        }
    }
}

