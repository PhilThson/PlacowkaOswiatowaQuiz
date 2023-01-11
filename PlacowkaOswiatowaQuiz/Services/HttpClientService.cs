using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.DTOs;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;
using PlacowkaOswiatowaQuiz.Helpers;

namespace PlacowkaOswiatowaQuiz.Services
{
	public class HttpClientService : IHttpClientService
	{
        #region Pola prywatne
        private readonly HttpClient _client;
        private readonly QuizApiSettings _apiSettings;
        private readonly IDictionary<Type, string> _endpoints;
        #endregion

        #region Konstruktor
        public HttpClientService(
            QuizApiUrl apiUrl,
            QuizApiSettings apiSettings,
            IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient(apiUrl.ClientName);
            _apiSettings = apiSettings;
            _endpoints = GetEndpoints();
        }
        #endregion

        #region Methods

        #region Pobranie wszystkich rekordów
        public async Task<List<T>> GetAllItems<T>()
        {
            if (!_endpoints.TryGetValue(typeof(T), out string endpoint))
                throw new DataNotFoundException();

            var response = await _client.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return JsonConvert.DeserializeObject<List<T>>(content);
        }
        #endregion

        #region Pobieranie rekordu po identyfikatorze
        public async Task<T> GetItemById<T>(object id)
        {
            if (!_endpoints.TryGetValue(typeof(T), out string endpoint))
                throw new DataNotFoundException();

            var url = $"{endpoint}/{id}";
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return JsonConvert.DeserializeObject<T>(content);
        }
        #endregion

        #region Pobranie rekordu po kluczu i wartości
        public async Task<T> GetItemByKey<T>(string key, string value)
        {
            if (!_endpoints.TryGetValue(typeof(T), out string endpoint))
                throw new DataNotFoundException();

            var url = $"{endpoint}?{key}={value}";
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return JsonConvert.DeserializeObject<T>(content);
        }
        #endregion

        #region Usuwanie rekordu po identyfikatorze
        public async Task RemoveItemById<T>(object id)
        {
            if (!_endpoints.TryGetValue(typeof(T), out string endpoint))
                throw new DataNotFoundException();

            var url = $"{endpoint}/{id}";
            var response = await _client.DeleteAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                    content = response.ReasonPhrase;
                throw new HttpRequestException(content);
            }
        }
        #endregion

        #region Dodawanie rekordu
        public async Task AddItem<T>(T item, string dict = null)
        {
            if (!_endpoints.TryGetValue(typeof(T), out string endpoint))
            {
                if (!string.IsNullOrEmpty(dict))
                    endpoint = dict;
                else
                    throw new DataNotFoundException();
            }

            var dataToSend = new StringContent(JsonConvert.SerializeObject(item),
                Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(endpoint, dataToSend);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);
        }
        #endregion

        #endregion

        #region Private methods

        private Dictionary<Type, string> GetEndpoints() =>
            new Dictionary<Type, string>
            {
                { typeof(EmployeeViewModel), _apiSettings.Employees },
                { typeof(StudentViewModel), _apiSettings.Students },
                { typeof(QuestionsSetViewModel), _apiSettings.QuestionsSets },
                { typeof(CreateQuestionsSetDto), _apiSettings.QuestionsSets },
                { typeof(QuestionViewModel), _apiSettings.Questions },
                { typeof(DiagnosisViewModel), _apiSettings.Diagnosis },
                { typeof(CreateDiagnosisDto), _apiSettings.Diagnosis },
                { typeof(AreaViewModel), _apiSettings.Areas },
                { typeof(DifficultyViewModel), _apiSettings.Difficulties },
                { typeof(AttachmentViewModel), _apiSettings.Attachments },
                { typeof(AttachmentFileViewModel), _apiSettings.Attachments },
                { typeof(RatingViewModel), _apiSettings.Ratings },
                { typeof(ResultViewModel), _apiSettings.Results },
                { typeof(ReportDto), _apiSettings.Reports },
            };

        #endregion
    }
}

