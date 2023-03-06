using System;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Text;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.DTOs;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using Newtonsoft.Json.Linq;

namespace PlacowkaOswiatowaQuiz.Services
{
	public class UserService : IUserService
	{
		private readonly HttpClient _httpClient;
        private readonly QuizApiSettings _apiSettings;

        public UserService(HttpClient httpClient, QuizApiSettings apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings;
        }

        public async Task<IEnumerable<string>> Login(SimpleUserDto simpleUser)
        {
            var dataToSend = new StringContent(JsonConvert.SerializeObject(simpleUser),
                Encoding.UTF8, "application/json");

            var response =
                await _httpClient.PostAsync(_apiSettings.User.Login, dataToSend);

            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            var cookie = response.Headers.First(h => h.Key == "Set-Cookie").Value;
            return cookie;
        }

        public async Task<SimpleUserDto> GetByEmail(string email)
        {
            var response = await _httpClient.GetAsync(
                _apiSettings.User.ByEmail + "?email=" + email);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return JsonConvert.DeserializeObject<SimpleUserDto>(content);
        }

        public async Task Register(CreateUserDto createUser)
        {
            var dataToSend = new StringContent(JsonConvert.SerializeObject(createUser),
                Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("", dataToSend);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);
        }

        public async Task<string> GetData()
        {
            var response = await _httpClient.GetAsync(_apiSettings.User.Data);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);
            return content;
        }
    }
}

