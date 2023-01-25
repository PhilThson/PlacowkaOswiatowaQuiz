using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class QuestionController : Controller
    {
        #region Pola prywatne
        private readonly QuizApiUrl _apiUrl;
        private readonly QuizApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpClientService _httpClient;
        #endregion

        #region Konstruktor
        public QuestionController(QuizApiSettings apiSettings,
            IHttpClientFactory httpClientFactory,
            QuizApiUrl apiUrl,
            IHttpClientService httpClient)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
            _apiUrl = apiUrl;
            _httpClient = httpClient;
        }
        #endregion

        #region Wszystkie pytania
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var questions = await _httpClient.GetAllItems<QuestionViewModel>();
            return View(questions);
        }
        #endregion

        #region Edycja pytania
        [HttpGet]
        public async Task<IActionResult> Edit(int id, int questionsSetId)
        {
            var question = new QuestionViewModel
            {
                QuestionsSetId = questionsSetId,
                IsFromQuestionsSet = questionsSetId != 0
            };
            try
            {
                if (id == default(int))
                    return View(question);

                var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);

                var response = await httpClient.GetAsync(
                    $"{_apiSettings.Questions}/{id}");

                response.EnsureSuccessStatusCode();

                question = await response.Content
                    .ReadFromJsonAsync<QuestionViewModel>();
                _ = question ?? throw new InvalidCastException(
                    "Nie udało się odczytać pytania");
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
            if (questionVM.QuestionsSetId == default(int))
                ModelState.AddModelError(string.Empty,
                    "Należy wskazać w skład którego zestawu wejdzie pytanie");
            
            if (!ModelState.IsValid)
                return View(questionVM);

            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = new HttpResponseMessage();

            if (questionVM.Id == default(int))
                response = await httpClient.PostAsJsonAsync(_apiSettings.Questions,
                    questionVM);
            else
                response = await httpClient.PutAsJsonAsync(_apiSettings.Questions,
                    questionVM);

            if (!response.IsSuccessStatusCode)
            {
                var responseMessage = await response.Content.ReadAsStringAsync();
                TempData["errorAlert"] = "Nieudana próba edycji/utworzenia pytania." +
                    $"Odpowiedź serwera: '{responseMessage}'";
                return View(questionVM);
            }

            TempData["successAlert"] = "Poprawnie zaktualizowano/dodano pytanie";

            if (questionVM.IsFromQuestionsSet)
                return RedirectToAction("Details", "QuestionsSet",
                    new { id = questionVM.QuestionsSetId });

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Usuwania pytania
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