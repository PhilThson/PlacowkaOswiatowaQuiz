using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Helpers.Filters;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class StudentController : Controller
    {
        #region Pola prywatne
        private readonly IHttpClientService _httpClient;
        #endregion

        #region Konstruktor
        public StudentController(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }
        #endregion

        #region Pobranie wszystkich uczniów
        public async Task<IActionResult> Index()
        {
            var students = new List<StudentViewModel>();
            try
            {
                students = await _httpClient.GetAllItems<StudentViewModel>();
                return View(students);
            }
            catch(Exception e)
            {
                TempData["errorAlert"] =
                    $"'Nie udało się pobrać wszystkich uczniów. {e.Message}'";
                return View(students);
            }
        }

        //Do pobieranie asynchronicznego
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _httpClient.GetAllItems<StudentViewModel>();
                return Ok(students);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    }
}

