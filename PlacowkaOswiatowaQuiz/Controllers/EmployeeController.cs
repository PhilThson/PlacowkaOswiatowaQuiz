﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Models.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly QuizApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public EmployeeController(QuizApiSettings apiSettings,
            IHttpClientFactory httpClientFactory)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var employees = new List<EmployeeViewModel>();

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);

            var uri = new Uri(_apiSettings.Employees);

            var response = await httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            employees = await response.Content
                .ReadFromJsonAsync<List<EmployeeViewModel>>();

            return View(employees);
        }
    }
}
