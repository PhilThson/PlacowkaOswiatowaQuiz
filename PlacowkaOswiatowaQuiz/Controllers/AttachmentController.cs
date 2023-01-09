using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Shared.DTOs;
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

        #region Pobranie załadowanego załącznika
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
        #endregion

        #region Pobranie załącznika po identyfikatorze
        public async Task<IActionResult> GetAttachmentById([FromQuery] int attachmentId)
        {
            var attachment = new AttachmentFileViewModel();
            try
            {
                var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
                var response = await httpClient.GetAsync(
                    $"{_apiSettings.Attachments}/{attachmentId}");

                response.EnsureSuccessStatusCode();
                attachment = await response.Content
                    .ReadFromJsonAsync<AttachmentFileViewModel>();

                if(string.IsNullOrEmpty(attachment.ContentType) ||
                    !attachment.ContentType.Contains("image"))
                {
                    TempData["errorAlert"] = $"Nie można wyświetlić pliku o " +
                        $"zadanym typie: {attachment.ContentType}";
                    return NoContent();
                }
                return File(attachment.Content, attachment.ContentType,
                    attachment.Name);
            }
            catch(HttpRequestException e)
            {
                TempData["errorAlert"] = $"Nie udało się pobrać pliku o " +
                    $"identyfikatorze {attachmentId}. Odpowiedź serwera: " +
                    $"'{e.Message}'";
                return NoContent();
            }
        }
        #endregion
    }
}

