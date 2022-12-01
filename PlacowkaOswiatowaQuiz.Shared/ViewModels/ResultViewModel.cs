using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class ResultViewModel
	{
        public long Id { get; set; }
        [DisplayName("Ocena zestawu pytań:")]
        [Required(ErrorMessage = "Należy wybrać ocenę zestawu pytań")]
        public RatingViewModel? QuestionsSetRating { get; set; }
        [DisplayName("Poziom oceny zestawu pytań:")]
        [Required(ErrorMessage = "Należy wybrać poziom oceny")]
        [Range(1, 6)]
        public byte? RatingLevel { get; set; }
        [DisplayName("Notatki:")]
        [MaxLength(2048)]
        public string? Notes { get; set; }
        public int DiagnosisId { get; set; }
    }
}