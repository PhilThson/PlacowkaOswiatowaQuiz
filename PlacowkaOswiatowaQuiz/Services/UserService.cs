using Newtonsoft.Json;
using System.Text;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.DTOs;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Helpers;

namespace PlacowkaOswiatowaQuiz.Services
{
    public class UserService : IUserService
	{
		private readonly HttpClient _httpClient;
        private readonly User _userController;

        public UserService(HttpClient httpClient, QuizApiSettings apiSettings)
        {
            _httpClient = httpClient;
            _userController = apiSettings.User;
        }

        public async Task<IEnumerable<string>> Login(SimpleUserDto simpleUser)
        {
            var encryptedUser = SecurePasswordHasher.Encrypt(simpleUser);
            var dataToSend = new StringContent(JsonConvert.SerializeObject(encryptedUser),
                Encoding.UTF8, "application/json");

            var response =
                await _httpClient.PostAsync(_userController.Login, dataToSend);

            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            var cookie = response.Headers.First(h => h.Key == "Set-Cookie").Value;
            return cookie;
        }

        public async Task<SimpleUserDto?> GetByEmail(string email)
        {
            var response = await _httpClient.GetAsync(
                _userController.ByEmail + "?email=" + email);
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

            var response = await _httpClient.PostAsync(string.Empty, dataToSend);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);
        }

        public async Task<string> GetData()
        {
            var response = await _httpClient.GetAsync(_userController.Data);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return content;
        }

        public async Task Logout()
        {
            var response = await _httpClient.PostAsync(_userController.Logout, null);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);
        }
    }
}

