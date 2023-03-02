using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class LoginViewModel
	{
		[DisplayName("E-mail:")]
		[Required(ErrorMessage = "Adres email jest wymagany")]
		[StringLength(20, ErrorMessage = "Adres email może mieć maksymalnie 20 znaków")]
		public string? Email { get; set; }

		[DisplayName("Hasło:")]
        [StringLength(20, ErrorMessage = "Hasło może mieć maksymalnie 20 znaków")]
        [Required(ErrorMessage = "Hasło jest wymagane")]
        public string? Password { get; set; }
	}
}

