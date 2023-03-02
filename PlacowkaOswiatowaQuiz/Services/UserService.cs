using System;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Text;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.DTOs;

namespace PlacowkaOswiatowaQuiz.Services
{
	public class UserService : IUserService
	{
		private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetUserToken()
        {
            var rsaKey = RSA.Create();
            var token = new TokenDto() { PrivateKey = rsaKey.ExportRSAPrivateKey() };
            var dataToSend = new StringContent(JsonConvert.SerializeObject(token),
                Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("user/token", dataToSend);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            //Tutaj powinno nastąpić cachowanie tokena
            return content;
        }

        public async Task<string> AuthenticateWithUserToken(string token)
        {
            //Uwierzytelnienie przy pomocy tokena
            var response = await _httpClient.GetAsync("user/auth?token=" + token);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}

