using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class QuestionViewModel
	{
        public int Id { get; set; }

        [Required(ErrorMessage = "Treść pytania jest wymagana")]
        [MaxLength(2048, ErrorMessage = "Przekroczono dopuszczalny limit znaków ({0})")]
        [DisplayName("Treść pytania")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Opis pytania jest wymagany")]
        [MaxLength(2048, ErrorMessage = "Przekroczono dopuszczalny limit znaków ({0})")]
        [DisplayName("Opis pytania (co robi nauczyciel)")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Należy wskazać zestaw pytań w skład którego wejdzie pytanie")]
        [DisplayName("Zestaw pytań")]
        public int QuestionsSetId { get; set; }
        public bool IsFromQuestionsSet { get; set; }
    }
}