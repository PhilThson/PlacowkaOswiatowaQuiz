using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Shared.DTOs;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class QuestionsSetController : Controller
    {
        private readonly QuizApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public QuestionsSetController(QuizApiSettings apiSettings,
            IHttpClientFactory httpClientFactory)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
        }

        #region QuestionsSet
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var questionsSets = new List<QuestionsSetViewModel>();
            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);
            var response = await httpClient.GetAsync(_apiSettings.QuestionsSets);
            response.EnsureSuccessStatusCode();
            questionsSets = await response.Content
                .ReadFromJsonAsync<List<QuestionsSetViewModel>>();

            return View(questionsSets);
        }

        public async Task<IActionResult> GetAllQuestionsSets()
        {
            var questionsSets = new List<QuestionsSetViewModel>();
            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);
            var response = await httpClient.GetAsync(_apiSettings.QuestionsSets);
            response.EnsureSuccessStatusCode();
            questionsSets = await response.Content
                .ReadFromJsonAsync<List<QuestionsSetViewModel>>();

            return Ok(questionsSets);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var questionsSet = new QuestionsSetViewModel();
            if (id == default(int))
                return View(questionsSet);
            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);
            var response = await httpClient.GetAsync(
                $"{_apiSettings.QuestionsSets}/{id}");
            response.EnsureSuccessStatusCode();
            questionsSet = await response.Content
                .ReadFromJsonAsync<QuestionsSetViewModel>();

            return View(questionsSet);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQuestionsSetViewModel questionsSetVM)
        {
            if (!ModelState.IsValid)
            {
                return View(questionsSetVM);
            }

            var createQuestionSetDto = new CreateQuestionsSetDto
            {
                SkillDescription = questionsSetVM.SkillDescription,
                AreaId = questionsSetVM.AreaId.GetValueOrDefault(),
                DifficultyId = questionsSetVM.DifficultyId.GetValueOrDefault(),
                Questions = questionsSetVM.Questions ?? new List<QuestionViewModel>(),
                QuestionsSetRatings = questionsSetVM.QuestionsSetRatings
            };

            if (questionsSetVM.AttachmentFiles?.Count() > 0)
            {
                var files = new List<AttachmentFileViewModel>();
                foreach(var file in questionsSetVM.AttachmentFiles)
                {
                    files.Add(new AttachmentFileViewModel
                    {
                        Name = file.Name,
                        Content = await GetBytes(file),
                        Size = file.Length,
                        ContentType = file.ContentType
                    });
                }
                createQuestionSetDto.AttachmentFiles = files;
            }

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);
            var response = await httpClient.PostAsJsonAsync(_apiSettings.QuestionsSets,
                createQuestionSetDto);

            if (!response.IsSuccessStatusCode)
            {
                TempData["errorAlert"] = "Nie udało się utworzyć zestawu pytań";
                return View(questionsSetVM);
            }
            TempData["successAlert"] = "Poprawnie utworzono zestaw pytań";
            return RedirectToAction(nameof(Index));
        }

        private async Task<byte[]> GetBytes(IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        #endregion

        #region Ratings
        [HttpPost]
        public async Task<IActionResult> EditRating(RatingViewModel ratingVM)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);
            var response = await httpClient.PutAsJsonAsync(_apiSettings.Ratings,
                ratingVM);

            if (!response.IsSuccessStatusCode)
                TempData["errorAlert"] = "Nieudana próba edycji oceny";
            else
                TempData["successAlert"] = "Poprawnie zaktualizowano ocenę";

            return RedirectToAction(nameof(Details), new { id = ratingVM.QuestionsSetId });
        }
        #endregion

        #region Skill
        [HttpPost]
        public async Task<IActionResult> EditSkill(int id, string skill)
        {
            if (!ModelState.IsValid || skill.Length == 0)
                return RedirectToAction(nameof(Details), new {id = id});

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);
            var response = await httpClient.PatchAsync(
                $"{_apiSettings.QuestionsSets}/{id}/skill?skill={skill}", null);
            var updated = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                TempData["errorAlert"] = "Nieudana próba edycji umiejętności";
            else
                TempData["successAlert"] = "Poprawnie zaktualizowano umiejętności";

            return RedirectToAction(nameof(Details), new { id = id });
        }
        #endregion

        #region QuestionsSet Area
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

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);
            var response = await httpClient.PatchAsync(
                $"{_apiSettings.QuestionsSets}/{id}/area?areaId={requestedAreaId}", null);

            if (!response.IsSuccessStatusCode)
                TempData["errorAlert"] = "Nieudana próba edycji obszaru zestawu pytań";
            else
                TempData["successAlert"] = "Poprawnie zaktualizowano obszar zestawu pytań";

            return RedirectToAction(nameof(Details), new { id = id });
        }
        #endregion

        #region QuestionsSet Difficulty
        [HttpPost]
        public async Task<IActionResult> EditDifficulty(int id, byte requestedDifficultyId,
            byte currentDifficultyId)
        {
            if (!ModelState.IsValid || requestedDifficultyId == default(byte))
            {
                TempData["errorAlert"] = "Niepoprawny format wprowadzonych danych";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            if(currentDifficultyId == requestedDifficultyId)
            {
                TempData["infoAlert"] = "Nie dokonano zmian";
                return RedirectToAction(nameof(Details), new { id = id });
            }    

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);
            var response = await httpClient.PatchAsync(
                $"{_apiSettings.QuestionsSets}/{id}/difficulty?difficultyId={requestedDifficultyId}",
                null);

            if (!response.IsSuccessStatusCode)
                TempData["errorAlert"] = "Nieudana próba zmiany skali trudności";
            else
                TempData["successAlert"] = "Poprawnie zaktualizowano skalę trudności";

            return RedirectToAction(nameof(Details), new { id = id });
        }
        #endregion
    }
}

