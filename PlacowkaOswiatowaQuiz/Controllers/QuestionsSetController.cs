﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using PlacowkaOswiatowaQuiz.Helpers.Extensions;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.DTOs;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class QuestionsSetController : Controller
    {
        #region Pola prywatne
        private readonly IHttpClientService _httpClient;
        #endregion

        #region Konstruktor
        public QuestionsSetController(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }
        #endregion

        #region Pobranie wszystkich zestawów pytań
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var questionsSets = new List<QuestionsSetViewModel>();
            try
            {
                questionsSets = await _httpClient.GetAllItems<QuestionsSetViewModel>();
                return View(questionsSets);
            }
            catch(HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nie udało się pobrać wszystkich" +
                    $"zestawów pytań. \nOdpowiedź serwera: '{e.Message}'";
                return View(questionsSets);
            }
        }

        public async Task<IActionResult> GetAllQuestionsSets()
        {
            try
            {
                var questionsSets = await _httpClient.GetAllItems<QuestionsSetViewModel>();
                return Ok(questionsSets);
            }
            catch(HttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region Szczegóły zestawu pytań
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var questionsSet = new QuestionsSetViewModel();
            if (id == default(int))
                return View(questionsSet);
            try
            {
                questionsSet = await _httpClient.GetItemById<QuestionsSetViewModel>(id);
                return View(questionsSet);
            }
            catch(HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nie udało się pobrać zestawu pytań." +
                    $"\nOdpowiedź serwera: '{e.Message}'";
                return View(questionsSet);
            }
        }
        #endregion

        #region Tworzenie zestawu pytań
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQuestionsSetViewModel questionsSetVM)
        {
            if(questionsSetVM.AttachmentFiles?.Count() > 0 &&
                questionsSetVM.AttachmentFiles.Any(a => !a.IsImage()))
                ModelState.AddModelError(string.Empty,
                    "Należy wybrać tylko pliki graficzne " +
                    "(.jpg/.png/.gif/.jpeg)");

            if (!ModelState.IsValid)
                return View(questionsSetVM);

            try
            {
                var ratings = questionsSetVM.QuestionsSetRatings
                    .Where(r => r != null).ToList();

                var createQuestionSetDto = new CreateQuestionsSetDto
                {
                    SkillDescription = questionsSetVM.SkillDescription,
                    AreaId = questionsSetVM.AreaId.Value, //właściwość wymagana
                    DifficultyId = questionsSetVM.DifficultyId.Value, //właściwość wymagana
                    Questions = questionsSetVM.Questions ?? new List<QuestionViewModel>(),
                    QuestionsSetRatings = ratings
                };

                if (questionsSetVM.AttachmentFiles?.Count() > 0)
                {
                    var files = new List<AttachmentFileViewModel>();
                    foreach (var file in questionsSetVM.AttachmentFiles)
                    {
                        files.Add(new AttachmentFileViewModel
                        {
                            Name = file.FileName,
                            Content = await GetBytes(file),
                            Size = file.Length,
                            ContentType = file.ContentType
                        });
                    }
                    createQuestionSetDto.AttachmentFiles = files;
                }

                await _httpClient.AddItem(createQuestionSetDto);

                TempData["successAlert"] = "Poprawnie utworzono zestaw pytań";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["errorAlert"] = $"Nie udało się utworzyć zestawu pytań." +
                    $"'{e.Message}'";
                return View(questionsSetVM);
            }
        }

        private async Task<byte[]> GetBytes(IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        #endregion

        #region Usuwanie zestawu pytań
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == default)
                return BadRequest("Nie znaleziono rekordu o podanym identyfikatorze");
            try
            {
                await _httpClient.DeleteItemById<QuestionsSetViewModel>(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region Wszystkie oceny zestawu pytań
        public async Task<IActionResult> GetQuestionsSetRatings(int questionsSetId)
        {
            try
            {
                var ratings = new List<RatingViewModel>();
                ratings = await _httpClient.GetAllItems<RatingViewModel>(
                    (nameof(questionsSetId), questionsSetId));

                return Ok(ratings);
            }
            catch(HttpRequestException e)
            {
                TempData["errorAlert"] = "Błąd podczas pobierania ocen zestawu pytań." +
                    $"Odpowiedź serwera: '{e.Message}";
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region Edycja oceny zestawu pytań
        [HttpPost]
        public async Task<IActionResult> EditRating(RatingViewModel ratingVM)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Details), new { id = ratingVM.QuestionsSetId});

            try
            {
                await _httpClient.UpdateItem<RatingViewModel>(ratingVM);

                TempData["successAlert"] = "Poprawnie zaktualizowano ocenę";
                return RedirectToAction(nameof(Details), new { id = ratingVM.QuestionsSetId });
            }
            catch(Exception e)
            {
                TempData["errorAlert"] = "Nieudana próba edycji oceny." +
                    $"Odpowiedź serwera: '{e.Message}";
                return RedirectToAction(nameof(Details), new { id = ratingVM.QuestionsSetId });
            }
        }
        #endregion

        #region Edycja umiejętności zestawu pytań
        [HttpPost]
        public async Task<IActionResult> EditSkill(int id, string skill)
        {
            if (!ModelState.IsValid || skill.Length == 0)
                return RedirectToAction(nameof(Details), new {id = id });

            try
            {
                var updated = await _httpClient.UpdateItemProperty<QuestionsSetViewModel>(
                    id, new KeyValuePair<string, string>(nameof(skill), skill));

                TempData["successAlert"] = "Poprawnie zaktualizowano umiejętności";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            catch(Exception e)
            {
                TempData["errorAlert"] = "Nieudana próba edycji umiejętności." +
                    $"Odpowiedź serwera: '{e.Message}";
                return RedirectToAction(nameof(Details), new { id = id });
            }
        }
        #endregion

        #region Edycja obszaru zestawu pytań
        [HttpPost]
        public async Task<IActionResult> EditArea(int id, byte requestedAreaId,
            byte currentAreaId)
        {
            if (!ModelState.IsValid || requestedAreaId == default(byte))
            {
                TempData["errorAlert"] = "Niepoprawny format wprowadzonych danych";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            if (currentAreaId == requestedAreaId)
            {
                TempData["infoAlert"] = "Nie dokonano zmian";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            try
            {
                await _httpClient.UpdateItemProperty<QuestionsSetViewModel>(id,
                    new KeyValuePair<string, string>("area", requestedAreaId.ToString()));

                TempData["successAlert"] = "Poprawnie zaktualizowano obszar zestawu pytań";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            catch(Exception e)
            {
                TempData["errorAlert"] = "Nieudana próba edycji obszaru zestawu pytań." +
                    $"Odpowiedź serwera: '{e.Message}";
                return RedirectToAction(nameof(Details), new { id = id });
            }
        }
        #endregion

        #region Edycja skali trudności zestawu pytań
        [HttpPost]
        public async Task<IActionResult> EditDifficulty(int id, byte requestedDifficultyId,
            byte currentDifficultyId)
        {
            if (!ModelState.IsValid || requestedDifficultyId == default(byte))
            {
                TempData["errorAlert"] = "Niepoprawny format wprowadzonych danych";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            if (currentDifficultyId == requestedDifficultyId)
            {
                TempData["infoAlert"] = "Nie dokonano zmian";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            try
            {
                await _httpClient.UpdateItemProperty<QuestionsSetViewModel>(id,
                    new KeyValuePair<string, string>("difficulty", requestedDifficultyId.ToString()));

                TempData["successAlert"] = "Poprawnie zaktualizowano skalę trudności";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            catch(Exception e)
            {
                TempData["errorAlert"] = "Nieudana próba zmiany skali trudności." +
                    $"Odpowiedź serwera: '{e.Message}";
                return RedirectToAction(nameof(Details), new { id = id });
            }
        }
        #endregion
    }
}

