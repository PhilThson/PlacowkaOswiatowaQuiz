﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using PlacowkaOswiatowaQuiz.Helpers;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class QuestionController : Controller
    {
        #region Pola prywatne
        private readonly IHttpClientService _httpClient;
        #endregion

        #region Konstruktor
        public QuestionController(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }
        #endregion

        #region Wszystkie pytania
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var questions = new List<QuestionViewModel>();
            try
            {
                questions = await _httpClient.GetAllItems<QuestionViewModel>();
                return View(questions);
            }
            catch(Exception e)
            {
                TempData["errorAlert"] = $"Nie udało się pobrać wszystkich pytań. " +
                    $"'{e.Message}'";
                return View(questions);
            }
        }
        #endregion

        #region Edycja/Tworzenie pytania
        [HttpGet]
        public async Task<IActionResult> Edit(int id, int questionsSetId)
        {
            var question = new QuestionViewModel
            {
                QuestionsSetId = questionsSetId,
                IsFromQuestionsSet = questionsSetId != 0
            };

            if (id == default(int))
                return View(question);

            try
            {
                question = await _httpClient.GetItemById<QuestionViewModel>(id);
                question.IsFromQuestionsSet = questionsSetId != 0;

                return View(question);
            }
            catch(Exception e)
            {
                TempData["errorAlert"] = e.Message;
                return View(question);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(QuestionViewModel questionVM)
        {
            if (!ModelState.IsValid)
                return View(questionVM);

            try
            {
                if (questionVM.Id == default(int))
                    await _httpClient.AddItem(questionVM);
                else
                    await _httpClient.UpdateItem(questionVM);

                TempData["successAlert"] = "Poprawnie zaktualizowano/dodano pytanie";

                if (questionVM.IsFromQuestionsSet)
                    return RedirectToAction("Details", "QuestionsSet",
                        new { id = questionVM.QuestionsSetId });

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                TempData["errorAlert"] = "Nieudana próba edycji/utworzenia pytania. " +
                    $"'{e.Message}'";
                return View(questionVM);
            }
        }
        #endregion

        #region Usuwania pytania
        //Do zapytań asynchronicznych
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == default)
                return BadRequest("Nie znaleziono rekordu o podanym identyfikatorze");
            try
            {
                await _httpClient.DeleteItemById<QuestionViewModel>(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    }
}