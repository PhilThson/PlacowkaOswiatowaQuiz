using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
//QueryHelpers
using Microsoft.Extensions.Options;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Models.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly QuizApiSettings _apiSettings;

        public EmployeeController(QuizApiSettings apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public async Task<IActionResult> Index()
        {
            var employees = new List<EmployeeViewModel>();
            using (var client = new HttpClient())
            {
                var uri = new Uri(
                    $"{_apiSettings.Host}" + "/" +
                    $"{_apiSettings.MainController}" + "/" +
                    $"{_apiSettings.Employees}");

                var response = await client.GetAsync(uri);

                employees = await response.Content
                    .ReadFromJsonAsync<List<EmployeeViewModel>>();
            }

            return View(employees);
        }
    }
}
