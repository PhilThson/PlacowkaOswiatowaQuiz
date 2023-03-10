using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Helpers;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Models;
using PlacowkaOswiatowaQuiz.Services;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;
using System.Diagnostics;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains(Constants.ErrorMessageKey))
            {
                TempData["errorAlert"] =
                    HttpContext.Session.GetString(Constants.ErrorMessageKey);

                HttpContext.Session.Remove(Constants.ErrorMessageKey);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        } 
    }
}