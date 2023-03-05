using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class RegisterViewModel
	{
        [DisplayName("Imię:")]
        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(50, ErrorMessage = "{0} musi posiadać nie więcej niż {1} znaków")]
        public string? FirstName { get; set; }

        [DisplayName("Nazwisko:")]
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(50, ErrorMessage = "{0} musi posiadać nie więcej niż {1} znaków")]
        public string? LastName { get; set; }

        [DisplayName("Adres e-mail:")]
        [Required(ErrorMessage = "Adres E-mail jest wymagany")]
        [StringLength(50)]
        public string? Email { get; set; }

        [DisplayName("Hasło:")]
        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(50, ErrorMessage = "{0} musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
        public string? Password { get; set; }

        [DisplayName("Powtórz hasło:")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Podane hasła są różne")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Rola jest wymagana")]
        public byte? RoleId { get; set; }
    }
}

