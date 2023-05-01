using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.DTOs;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class AttachmentController : Controller
    {
        #region Pola prywatne
        private readonly IHttpClientService _httpClient;
        #endregion

        #region Konstruktor
        public AttachmentController(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }
        #endregion

        #region Pobranie załadowanego załącznika (ze szczegółów zestawu pytań)
        public async Task<IActionResult> Download([FromQuery] int attachmentId)
        {
            var attachment = new AttachmentFileViewModel();
            if (attachmentId == default(int))
                return NotFound("Nie znaleziono karty pracy o podanym identyfikatorze");
            try
            {
                attachment =
                    await _httpClient.GetItemById<AttachmentFileViewModel>(attachmentId);

                return File(attachment.Content, "application/octet-stream", attachment.Name);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region Pobranie załącznika po identyfikatorze
        public async Task<IActionResult> GetAttachmentById([FromQuery] int attachmentId)
        {
            var attachment = new AttachmentFileViewModel();
            try
            {
                attachment =
                    await _httpClient.GetItemById<AttachmentFileViewModel>(attachmentId);

                if (string.IsNullOrEmpty(attachment.ContentType) ||
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
                //Komunikat z TempData zostanie wyświetlony po odświeżeniu strony
                TempData["errorAlert"] = $"Nie udało się pobrać pliku o " +
                    $"identyfikatorze {attachmentId}. Odpowiedź serwera: " +
                    $"'{e.Message}'";
                return NoContent();
            }
        }
        #endregion
    }
}

