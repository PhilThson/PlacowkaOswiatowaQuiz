using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class QuestionViewModel
	{
        public int Id { get; set; }
        [Required]
        [MaxLength(2048)]
        [DisplayName("Treść pytania")]
        public string Content { get; set; }
        [Required]
        [MaxLength(2048)]
        [DisplayName("Opis pytania")]
        public string Description { get; set; }
        [DisplayName("Zestaw pytań")]
        public int QuestionsSetId { get; set; }
    }
}