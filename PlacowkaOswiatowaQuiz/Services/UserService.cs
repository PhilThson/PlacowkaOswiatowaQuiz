using System;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Text;
using PlacowkaOswiatowaQuiz.Interfaces;

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
            var token = new Token() { PrivateKey = rsaKey.ExportRSAPrivateKey() };
            //var base64key = Convert.ToBase64String(privateKey);
            var dataToSend = new StringContent(JsonConvert.SerializeObject(token),
                Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("user/token", dataToSend);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();



            return content;
        }

        public async Task<string> AuthenticateWithUserToken(string token)
        {
            var response = await _httpClient.GetAsync("user/auth?token=" + token);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }

    class Token
    {
        public byte[] PrivateKey { get; set; }
    }
}

