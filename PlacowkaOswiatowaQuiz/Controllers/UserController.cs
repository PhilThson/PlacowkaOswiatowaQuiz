using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Shared.DTOs;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;
using Microsoft.AspNetCore.Identity;
using PlacowkaOswiatowaQuiz.Helpers;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Nieprawidłowy model logowania");

            try
            {
                var user = new SimpleUserDto
                {
                    Email = loginViewModel.Email,
                    Password = loginViewModel.Password
                };

                var cookie = await _userService.Login(user);
                HttpContext.Session.SetString("quiz-user", cookie.First());

                return Ok("Zalogowano!");
            }
            catch (Exception e)
            {
                return BadRequest($"Wystąpił błąd: '{e.Message}'");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
                return View(register);

            try
            {
                var createUser = new CreateUserDto
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Email = register.Email,
                    PasswordHash = SecurePasswordHasher.Hash(register.Password),
                    RoleId = register.RoleId.Value
                };

                await _userService.Register(createUser);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                TempData["errorAlert"] = $"Nie udało się utworzyć użytkownika." +
                    $"\nOdpowiedź serwera: '{e.Message}'";

                return View(register);
            } 
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                //odpytanie endpointu do wylogowania,

                //wyczyszczenie sesji

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest($"Wystąpił błąd: '{e.Message}'");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Data()
        {
            try
            {
                var response = await _userService.GetData();
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

