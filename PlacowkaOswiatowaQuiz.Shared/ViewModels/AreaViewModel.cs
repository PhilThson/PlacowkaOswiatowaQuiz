using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class AreaViewModel
	{
        public byte Id { get; set; }
        [Required]
        [MaxLength(512, ErrorMessage = "Nazwa może mieć maksymalnie 512 znaków")]
        [DisplayName("Nazwa skrócona")]
        public string Name { get; set; }
        [MaxLength(1024)]
        [DisplayName("Opis")]
        public string? Description { get; set; }
        [MaxLength(1024)]
        [DisplayName("Nazwa rozszerzona")]
        public string? ExtendedName { get; set; }
    }
}

