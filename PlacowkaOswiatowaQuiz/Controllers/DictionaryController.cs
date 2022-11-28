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
    public class DictionaryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly QuizApiSettings _apiSettings;
        private readonly QuizApiUrl _apiUrl;

        public DictionaryController(IHttpClientFactory httpClientFactory,
            QuizApiSettings apiSettings,
            QuizApiUrl apiUrl)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings;
            _apiUrl = apiUrl;
        }

        #region Area
        public async Task<IActionResult> GetAreas()
        {
            var areas = new List<AreaViewModel>();
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(_apiSettings.Areas);
            response.EnsureSuccessStatusCode();
            areas = await response.Content
                .ReadFromJsonAsync<List<AreaViewModel>>();

            return Ok(areas);
        }

        public async Task<IActionResult> IndexArea()
        {
            var areas = new List<AreaViewModel>();
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(_apiSettings.Areas);
            response.EnsureSuccessStatusCode();
            areas = await response.Content
                .ReadFromJsonAsync<List<AreaViewModel>>();

            return View(areas);
        }

        public async Task<IActionResult> EditArea(byte id)
        {
            var area = new AreaViewModel();
            if (id == default(byte))
                return BadRequest();

            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(
                $"{_apiSettings.Areas}/{id}");
            response.EnsureSuccessStatusCode();
            area = await response.Content
                .ReadFromJsonAsync<AreaViewModel>();

            return View(area);
        }

        [HttpPost]
        public async Task<IActionResult> EditArea(AreaViewModel areaVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.PutAsJsonAsync(_apiSettings.Areas,
                    areaVM);

            if (!response.IsSuccessStatusCode)
            {
                //Dodanie informacji (ViewBag) że operacja się nie powiodła
                TempData["errorAlert"] = "Nieudana próba edycji obszaru";
                return View(areaVM);
            }

            TempData["successAlert"] = "Poprawnie zaktualizowano obszar";
            return RedirectToAction(nameof(IndexArea));
        }
        #endregion

        #region Difficulty
        public async Task<IActionResult> GetDifficulties()
        {
            var difficulties = new List<DifficultyViewModel>();
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(_apiSettings.Difficulties);
            response.EnsureSuccessStatusCode();
            difficulties = await response.Content
                .ReadFromJsonAsync<List<DifficultyViewModel>>();

            return Ok(difficulties);
        }

        public async Task<IActionResult> IndexDifficulty()
        {
            var difficulties = new List<DifficultyViewModel>();
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(_apiSettings.Difficulties);
            response.EnsureSuccessStatusCode();
            difficulties = await response.Content
                .ReadFromJsonAsync<List<DifficultyViewModel>>();

            return View(difficulties);
        }

        public async Task<IActionResult> EditDifficulty(byte id)
        {
            var difficulty = new DifficultyViewModel();
            if (id == default(byte))
                return View(difficulty);

            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(
                $"{_apiSettings.Difficulties}/{id}");
            response.EnsureSuccessStatusCode();
            difficulty = await response.Content
                .ReadFromJsonAsync<DifficultyViewModel>();

            return View(difficulty);
        }

        [HttpPost]
        public async Task<IActionResult> EditDifficulty(DifficultyViewModel difficultyVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.PutAsJsonAsync(_apiSettings.Difficulties,
                    difficultyVM);

            if (!response.IsSuccessStatusCode)
            {
                //Dodanie informacji (ViewBag) że operacja się nie powiodła
                TempData["errorAlert"] = "Nieudana próba edycji skali trudności";
                return View(difficultyVM);
            }

            TempData["successAlert"] = "Poprawnie zaktualizowano skalę trudności";
            return RedirectToAction(nameof(IndexDifficulty));
        }
        #endregion
    }
}

