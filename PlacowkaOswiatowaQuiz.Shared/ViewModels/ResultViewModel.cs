using System;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class ResultViewModel
	{
        public long Id { get; set; }
        public RatingViewModel QuestionsSetRating { get; set; }
        [Range(1, 6)]
        public byte RatingLevel { get; set; }
        [MaxLength(2048)]
        public string? Notes { get; set; }
        public int DiagnosisId { get; set; }
    }
}

