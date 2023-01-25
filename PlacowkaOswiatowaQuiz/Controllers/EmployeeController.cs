using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Models.Controllers
{
    public class EmployeeController : Controller
    {
        #region Pola prywatne
        private readonly IHttpClientService _httpClient;
        #endregion

        #region Konstruktor
        public EmployeeController(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }
        #endregion

        #region Pobieranie wszystkich pracowników
        public async Task<IActionResult> Index()
        {
            var employees = new List<EmployeeViewModel>();
            try
            {
                employees = await _httpClient.GetAllItems<EmployeeViewModel>();
                return View(employees);
            }
            catch(Exception e)
            {
                TempData["errorAlert"] =
                    $"Nie udało się pobrać wszystkich pracowników. '{e.Message}'";
                return View(employees);
            }
        }

        //Do pobierania asynchronicznego
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await _httpClient.GetAllItems<EmployeeViewModel>();
                return Ok(employees);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        #endregion
    }
}
