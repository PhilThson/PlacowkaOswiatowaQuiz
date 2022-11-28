using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Models.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly QuizApiUrl _apiUrl;
        private readonly QuizApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public EmployeeController(QuizApiSettings apiSettings,
            IHttpClientFactory httpClientFactory,
            QuizApiUrl apiUrl)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
            _apiUrl = apiUrl;
        }

        public async Task<IActionResult> Index()
        {
            var employees = new List<EmployeeViewModel>();
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(_apiSettings.Employees);
            response.EnsureSuccessStatusCode();
            employees = await response.Content
                .ReadFromJsonAsync<List<EmployeeViewModel>>();

            return View(employees);
        }

        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = new List<EmployeeViewModel>();
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(_apiSettings.Employees);
            response.EnsureSuccessStatusCode();
            employees = await response.Content
                .ReadFromJsonAsync<List<EmployeeViewModel>>();

            return Ok(employees);
        }
    }
}
