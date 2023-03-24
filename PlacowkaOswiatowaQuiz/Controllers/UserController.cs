using Microsoft.AspNetCore.Mvc;
using PlacowkaOswiatowaQuiz.Shared.DTOs;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;
using PlacowkaOswiatowaQuiz.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace PlacowkaOswiatowaQuiz.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Nieprawidłowy model logowania");

            try
            {
                //Wszystkie właściwości są wymagane
                var user = new SimpleUserDto
                {
                    Email = loginViewModel.Email,
                    Password = loginViewModel.Password
                };

                var cookie = await _userService.Login(user);
                //zwracana jest kolekcja ciągów znakowych, gdyby został przekroczony
                //dopuszczalny rozmiar pojedynczej wartości klucza sesji
                HttpContext.Session.SetString(Constants.QuizUserKey, cookie.First());
                HttpContext.Session.SetString(Constants.UserEmailKey, user.Email);

                string returnUrl = "/";
                if (HttpContext.Session.Keys.Contains(Constants.ReturnUrlKey))
                {
                    returnUrl = HttpContext.Session.GetString(Constants.ReturnUrlKey) ??
                        returnUrl;
                    HttpContext.Session.Remove(Constants.ReturnUrlKey);
                }

                return Ok(new
                {
                    message = "Zalogowano!",
                    returnUrl = returnUrl
                });
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

                TempData["successAlert"] = "Pomyślnie dodano użytkownika";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                TempData["errorAlert"] = $"Nie udało się utworzyć użytkownika." +
                    $"\nOdpowiedź serwera: '{e.Message}'";
                _logger.LogError(e, "Błąd połączenia z serwerem");

                return View(register);
            } 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                if (!HttpContext.Session.Keys.Contains(Constants.QuizUserKey))
                    throw new DataValidationException(
                        "Nie znaleziono zalogowanego użytkownika");

                await _userService.Logout();

                HttpContext.Session.Clear();

                return Ok("Wylogowano");
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
                _logger.LogError(e, "Błąd połączenia z serwerem");
                return BadRequest(e.Message);
            }
        }
    }
}

