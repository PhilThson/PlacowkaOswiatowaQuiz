using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Shared.DTOs
{
	public class RatingDto
	{
        public int Id { get; set; }
        public string? RatingDescription { get; set; }
        public int QuestionsSetId { get; set; }
    }
}

