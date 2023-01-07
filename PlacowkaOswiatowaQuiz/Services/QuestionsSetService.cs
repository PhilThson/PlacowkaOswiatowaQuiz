using System;
using System.Net.Mime;
using System.Text;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;
using Newtonsoft.Json;

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

        public async Task<List<QuestionsSetViewModel>> GetAllQuestionsSets(
            byte? difficultyId = null)
        {
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var uri = difficultyId.HasValue ?
                $"{_apiSettings.QuestionsSets}?difficultyId={difficultyId}" :
                _apiSettings.QuestionsSets;
            var response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<QuestionsSetViewModel>>();
        }

        public async Task<List<QuestionsSetViewModel>> GetQuestionsSetsByIds(List<int> ids)
        {
            if (ids == null || ids.Count < 1)
                return null;

            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);

            var idsString = string.Join(',', ids);
            //wysyłanie żądania GET z Body (ale niezgodne z RestApi)
            //var request = new HttpRequestMessage
            //{
            //    Method = HttpMethod.Get,
            //    RequestUri = new Uri(httpClient.BaseAddress, _apiSettings.QuestionsSetsAsked),
            //    Content = new StringContent(
            //        JsonConvert.SerializeObject(ids),
            //        Encoding.UTF8,
            //        MediaTypeNames.Application.Json)
            //};

            //var response = await httpClient.SendAsync(request);
            var response = await httpClient.GetAsync(
                $"{_apiSettings.QuestionsSetsAsked}?askedQuestionSetsIds={idsString}");
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

        public async Task<List<RatingViewModel>> GetRatingsByQuestionsSetId(int id)
        {
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(
                $"{_apiSettings.Ratings}?questionsSetId={id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<RatingViewModel>>();
        }
    }
}

