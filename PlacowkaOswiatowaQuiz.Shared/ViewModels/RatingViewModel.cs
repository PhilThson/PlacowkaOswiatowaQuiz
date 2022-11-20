using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class RatingViewModel
	{
        public int Id { get; set; }
        [Required]
        [MaxLength(1024)]
        [DisplayName("Opis oceny zestawu pytań")]
        public string RatingDescription { get; set; }
        public int QuestionsSetId { get; set; }
    }
}

