﻿using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.DTOs;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Services
{
    public class DiagnosisService : IDiagnosisService
    {
        private readonly QuizApiUrl _apiUrl;
        private readonly QuizApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public DiagnosisService(QuizApiUrl apiUrl,
            QuizApiSettings apiSettings,
            IHttpClientFactory httpClientFactory)
        {
            _apiUrl = apiUrl;
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<DiagnosisViewModel>> GetAllDiagnosis()
        {
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(_apiSettings.Diagnosis);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<DiagnosisViewModel>>();
        }

        public async Task<DiagnosisViewModel> GetDiagnosisById(int id)
        {
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync($"{_apiSettings.Diagnosis}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<DiagnosisViewModel>();
        }

        public async Task<DiagnosisViewModel> CreateDiagnosis(
            CreateDiagnosisViewModel diagnosisVM)
        {
            var createDiagnosis = (CreateDiagnosisDto)diagnosisVM;

            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.PostAsJsonAsync(_apiSettings.Diagnosis,
                createDiagnosis);

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());

            return await response.Content.ReadFromJsonAsync<DiagnosisViewModel>();
        }

        public async Task CreateResult(ResultViewModel resultVM)
        {
            var createResultDto = new CreateResultDto
            {
                DiagnosisId = resultVM.DiagnosisId,
                RatingId = resultVM.QuestionsSetRating.Id,
                RatingLevel = resultVM.RatingLevel.Value,
                Notes = resultVM.Notes
            };

            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.PostAsJsonAsync(_apiSettings.Results,
                createResultDto);

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<ResultViewModel> GetResultById(int id)
        {
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync($"{_apiSettings.Results}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ResultViewModel>();
        }

        public async Task<ResultViewModel> GetResultByDiagnosisQuestionsSetIds(
            int diagnosisId, int questionsSetId)
        {
            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(
                $"{_apiSettings.Results}?diagnosisId={diagnosisId}" +
                $"&questionsSetId={questionsSetId}");

            if (!response.IsSuccessStatusCode)
                return new ResultViewModel() { DiagnosisId = diagnosisId };

            return await response.Content.ReadFromJsonAsync<ResultViewModel>();
        }

        public async Task<BaseReportDto> CreateDiagnosisReport(int diagnosisId)
        {
            if (diagnosisId == default)
                throw new Exception("Nie znaleziono diagnozy o podanym identyfikatorze " +
                    $"({diagnosisId})");

            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            httpClient.Timeout = TimeSpan.FromSeconds(70);
            var response = await httpClient.PostAsync(
                $"{_apiSettings.Reports}/{diagnosisId}", null);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BaseReportDto>();
        }

        public async Task<ReportDto> GetDiagnosisReportById(int reportId)
        {
            if (reportId == default)
                throw new Exception("Podano nieprawidłowy identyfikator raportu");

            var httpClient = _httpClientFactory.CreateClient(_apiUrl.ClientName);
            var response = await httpClient.GetAsync(
                $"{_apiSettings.Reports}/{reportId}");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReportDto>();
        }
    }
}

