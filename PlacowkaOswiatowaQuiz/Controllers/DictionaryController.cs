using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Helpers;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class DictionaryController : Controller
    {
        #region Pola prywatne
        private readonly IHttpClientService _httpClient;
        #endregion

        #region Konstruktor
        public DictionaryController(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }
        #endregion

        #region Obszar zestawu pytań
        //Do zapytań asynchronicznych
        public async Task<IActionResult> GetAreas()
        {
            try
            {
                //API zwraca pustą listę jeżeli jest brak elementów
                var areas = await _httpClient.GetAllItems<AreaViewModel>();
                return Ok(areas);
            }
            catch(HttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }

        public async Task<IActionResult> IndexArea()
        {
            var areas = new List<AreaViewModel>();
            try
            {
                areas = await _httpClient.GetAllItems<AreaViewModel>();
                return View(areas);
            }
            catch(HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nieudało się pobrać obszarów. {e.Message}";
                return View(areas);
            }
        }

        public async Task<IActionResult> EditArea(byte id)
        {
            try
            {
                if (id == default(byte))
                    throw new DataValidationException(
                        "Nie znaleziono obszaru o podanym identyfikatorze");

                var area = await _httpClient.GetItemById<AreaViewModel>(id);

                return View(area);
            }
            catch(Exception e)
            {
                TempData["errorAlert"] = e.Message;
                return RedirectToAction(nameof(IndexArea));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditArea(AreaViewModel areaVM)
        {
            if (!ModelState.IsValid)
                return View(areaVM);

            try
            {
                await _httpClient.UpdateItem(areaVM);

                TempData["successAlert"] = "Poprawnie zaktualizowano obszar";
                return RedirectToAction(nameof(IndexArea));
            }
            catch (Exception e)
            {
                TempData["errorAlert"] = "Nieudana próba edycji obszaru";
                return View(areaVM);
            }
        }
        #endregion

        #region Skala trudności
        //Do zapytań asynchronicznych
        public async Task<IActionResult> GetDifficulties()
        {
            try
            {
                var difficulties = await _httpClient.GetAllItems<DifficultyViewModel>();
                return Ok(difficulties);
            }
            catch(HttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }

        public async Task<IActionResult> IndexDifficulty()
        {
            var difficulties = new List<DifficultyViewModel>();
            try
            {
                difficulties = await _httpClient.GetAllItems<DifficultyViewModel>();
                return View(difficulties);
            }
            catch(HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nieudało się pobrać skal trudności. {e.Message}";
                return View(difficulties);
            }
        }

        public async Task<IActionResult> EditDifficulty(byte id)
        {
            try
            {
                var difficulty = new DifficultyViewModel();
                if (id == default(byte))
                    throw new DataNotFoundException(
                        "Nie znaleziono skali trudności o podanym identyfikatorze");

                difficulty = await _httpClient.GetItemById<DifficultyViewModel>(id);

                return View(difficulty);
            }
            catch(Exception e)
            {
                TempData["errorAlert"] = "Nieudana próba edycji skali trudności";
                return RedirectToAction(nameof(IndexDifficulty));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditDifficulty(DifficultyViewModel difficultyVM)
        {
            if (!ModelState.IsValid)
                return View(difficultyVM);

            try
            {
                await _httpClient.UpdateItem(difficultyVM);

                TempData["successAlert"] = "Poprawnie zaktualizowano skalę trudności";
                return RedirectToAction(nameof(IndexDifficulty));
            }
            catch (Exception e)
            {
                TempData["errorAlert"] = "Nieudana próba edycji skali trudności";
                return View(difficultyVM);
            }
        }
        #endregion
    }
}

