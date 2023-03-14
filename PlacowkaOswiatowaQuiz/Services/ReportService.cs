using System;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.DTOs;

namespace PlacowkaOswiatowaQuiz.Services
{
	public class ReportService : IReportService
	{
        #region Pola prywatne
        private readonly QuizApiUrl _apiUrl;
        private readonly QuizApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion

        #region Konstruktor
        public ReportService(QuizApiUrl apiUrl,
            QuizApiSettings apiSettings,
            IHttpClientFactory httpClientFactory)
        {
            _apiUrl = apiUrl;
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
        }
        #endregion

        #region Metody

        public async Task<BaseReportDto> CreateDiagnosisReport(int diagnosisId)
        {
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            httpClient.Timeout = TimeSpan.FromSeconds(70);
            var response = await httpClient.PostAsync(
                $"{_apiSettings.Data.Reports}/{diagnosisId}", null);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BaseReportDto>();
        }

        #endregion
    }
}

