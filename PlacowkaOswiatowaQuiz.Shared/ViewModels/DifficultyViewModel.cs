using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class DifficultyViewModel
	{
        public byte Id { get; set; }
        [Required(ErrorMessage = "Skala trudności musi posiadać nazwę")]
        [MaxLength(512, ErrorMessage = "Nazwa może mieć maksymalnie 512 znaków")]
        [DisplayName("Nazwa")]
        public string Name { get; set; }
        [MaxLength(1024)]
        [DisplayName("Opis")]
        public string? Description { get; set; }
    }
}

