using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class QuestionsSetViewModel
	{
        public int Id { get; set; }
        [Required(ErrorMessage = "Opis sprawdzanej umiejętności jest wymagany")]
        [MaxLength(2048)]
        [DisplayName("Opis umiejętności")]
        public string SkillDescription { get; set; }
        [Required(ErrorMessage = "Należy wybrać obszar zestawu pytań")]
        [DisplayName("Obszar zestawu pytań")]
        public AreaViewModel Area { get; set; }
        [Required(ErrorMessage = "Należy wybrać skalę trudności")]
        [DisplayName("Skala trudności")]
        public DifficultyViewModel Difficulty { get; set; }
        [Required(ErrorMessage = "Oceny zestawu pytań są wymagane")]
        [DisplayName("Oceny zestawu pytań")]
        public IEnumerable<RatingViewModel> QuestionsSetRatings { get; set; }
        [DisplayName("Pytania")]
        public IEnumerable<QuestionViewModel> Questions { get; set; }
        [DisplayName("Dołączone pliki - Karty pracy")]
        public IEnumerable<AttachmentViewModel>? Attachments { get; set; }
    }
}