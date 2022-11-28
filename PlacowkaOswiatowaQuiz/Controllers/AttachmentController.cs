using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class AttachmentController : Controller
    {
        private readonly QuizApiUrl _apiUrl;
        private readonly QuizApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public AttachmentController(QuizApiSettings apiSettings,
            IHttpClientFactory httpClientFactory,
            QuizApiUrl apiUrl)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
            _apiUrl = apiUrl;
        }

        public async Task<IActionResult> Download([FromQuery] int attachmentId)
        {
            var attachment = new AttachmentFileViewModel();
            if (attachmentId == default(int))
                return NotFound();

            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(
                $"{_apiSettings.Attachments}/{attachmentId}");

            response.EnsureSuccessStatusCode();
            attachment = await response.Content
                .ReadFromJsonAsync<AttachmentFileViewModel>();

            return File(attachment.Content, "application/octet-stream", attachment.Name);
        }
    }
}

