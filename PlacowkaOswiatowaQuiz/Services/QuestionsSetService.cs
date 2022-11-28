using System;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Services
{
	public class QuestionsSetService : IQuestionsSetService
	{
        private readonly QuizApiUrl _apiUrl;
        private readonly QuizApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public QuestionsSetService(QuizApiUrl apiUrl,
            QuizApiSettings apiSettings,
            IHttpClientFactory httpClientFactory)
        {
            _apiUrl = apiUrl;
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<QuestionsSetViewModel>> GetAllQuestionsSets()
        {
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(_apiSettings.QuestionsSets);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<QuestionsSetViewModel>>();
        }

        public async Task<QuestionsSetViewModel> GetQuestionsSetById(int id)
        {
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync($"{_apiSettings.QuestionsSets}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<QuestionsSetViewModel>();
        }
    }
}

