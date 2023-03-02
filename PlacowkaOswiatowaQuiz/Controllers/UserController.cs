using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Shared.DTOs;
using PlacowkaOswiatowaQuiz.Interfaces;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpClientService _httpClientService;

        public UserController(IUserService userService,
            IHttpClientService httpClientService)
        {
            _userService = userService;
            _httpClientService = httpClientService;
        }

        public async Task<IActionResult> Index()
        {
            var token = await _userService.GetUserToken();
            return Ok(token);
        }

        public async Task<IActionResult> Authenticate([FromQuery] string token)
        {
            var response = await _userService.AuthenticateWithUserToken(token);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            //Post do enpointu logowania


            //return Ok("Super!");
            return BadRequest("Źle!");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            //Odpytanie endpointu do wylogowania
            return NoContent();
        }
    }
}

