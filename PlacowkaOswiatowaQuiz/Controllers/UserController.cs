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

        public UserController(IUserService userService)
        {
            _userService = userService;
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
    }
}

