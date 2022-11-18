using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using PlacowkaOswiatowaQuiz.Helpers.Options;
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var questionsSets = new List<QuestionsSetViewModel>();

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);

            //var uri = new Uri($"{_apiSettings.QuestionsSets}");

            var response = await httpClient.GetAsync(_apiSettings.QuestionsSets);

            response.EnsureSuccessStatusCode();

            questionsSets = await response.Content
                .ReadFromJsonAsync<List<QuestionsSetViewModel>>();

            return View(questionsSets);
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

        [HttpPost]
        public async Task<IActionResult> Edit(QuestionsSetViewModel questionsSetVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //var json = JsonConvert.SerializeObject(questionVM);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);

            //var uri = new Uri(_apiSettings.QuestionsSets);

            var response = await httpClient.PostAsJsonAsync(_apiSettings.QuestionsSets,
                questionsSetVM);

            //response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode)
            {
                //Dodanie informacji (ViewBag) że operacja się nie powiodła
                return View(questionsSetVM);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DownloadAttachment([FromQuery] int attachmentId)
        {
            var attachment = new AttachmentFileViewModel();

            if (attachmentId == default(int))
                return NotFound();

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);

            var response = await httpClient.GetAsync(
                $"{_apiSettings.Attachments}/{attachmentId}");

            response.EnsureSuccessStatusCode();

            attachment = await response.Content
                .ReadFromJsonAsync<AttachmentFileViewModel>();

            return File(attachment.Content, "application/octet-stream", attachment.Name);
        }

        [HttpPost]
        public async Task<IActionResult> EditRating(RatingViewModel ratingVM)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var httpClient = _httpClientFactory.CreateClient(_apiSettings.ClientName);
            var response = await httpClient.PutAsJsonAsync(_apiSettings.Ratings,
                ratingVM);

            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Details), new { id = ratingVM.QuestionsSetId });
        }
    }
}

