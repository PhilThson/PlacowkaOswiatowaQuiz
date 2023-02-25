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

        #region Metody

        #region Pobranie wszystkich rekordów
        public async Task<List<T>> GetAllItems<T>(params (string, object)[] queryParams)
        {
            var url = GetUrl(typeof(T), queryParams);

            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return JsonConvert.DeserializeObject<List<T>>(content);
        }
        #endregion

        #region Pobieranie rekordu po identyfikatorze
        public async Task<T> GetItemById<T>(object? id,
            params (string, object)[] queryParams)
        {
            var url = GetUrl(typeof(T), queryParams, id);

            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return JsonConvert.DeserializeObject<T>(content);
        }
        #endregion

        #region Usuwanie rekordu po identyfikatorze
        public async Task DeleteItemById<T>(object id)
        {
            var url = GetUrl(typeof(T), null, id);

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
        public async Task<object> AddItem<T>(T item, params (string, object)[] queryParams)
        {
            var url = GetUrl(typeof(T), queryParams);

            var dataToSend = new StringContent(JsonConvert.SerializeObject(item),
                Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, dataToSend);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            if (!int.TryParse(response.Headers.Location.Segments.Last(), out int objectId))
                throw new DataNotFoundException(
                    "Nie udało się odczytać identyfikatora utworzonego obiektu");

            return objectId;
        }
        #endregion

        #region Aktualizacja rekordu
        public async Task UpdateItem<T>(T item, params (string, object)[] queryParams)
        {
            var url = GetUrl(typeof(T), queryParams);
            var dataToSend = new StringContent(JsonConvert.SerializeObject(item),
                Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, dataToSend);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);
        }
        #endregion

        #region Aktualizacja pojedynczej właściwości obiektu
        public async Task<string> UpdateItemProperty<T>(object itemId,
            KeyValuePair<string, string> propertyValue)
        {
            var url = GetUrl(typeof(T), null, itemId);
            var dataToSend = new StringContent(
                JsonConvert.SerializeObject(propertyValue),
                Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync(url, dataToSend);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return content;
        }
        #endregion

        #endregion

        #region Metody prywatne

        private string GetUrl(Type objectType,
            (string, object)[]? queryParams = null, object? id = null)
        {
            if (!_endpoints.TryGetValue(objectType, out string endpoint))
                throw new DataNotFoundException(
                    "Nie udało się odnaleźć endpointu dla obiektu o zadanym typie " +
                    $"{objectType}");

            var endpointBuilder = new StringBuilder(endpoint);
            if (id != null)
                endpointBuilder.Append($"/{id}");

            if (queryParams != null && queryParams.Length > 0)
                endpointBuilder.Append(ResolveQueryParams(queryParams));

            return endpointBuilder.ToString();
        }

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
                { typeof(CreateResultDto), _apiSettings.Results },
                { typeof(ReportDto), _apiSettings.Reports },
            };

        private StringBuilder ResolveQueryParams((string, object)[] queryParams)
        {
            var queryBuilder = new StringBuilder("?");

            for(int i = 0; i < queryParams.Length; i++)
            {
                queryBuilder.Append($"{queryParams[i].Item1}={queryParams[i].Item2}");
                if (i != queryParams.Length - 1)
                    queryBuilder.Append('&');
            }
            return queryBuilder;
        }

        #endregion
    }
}

