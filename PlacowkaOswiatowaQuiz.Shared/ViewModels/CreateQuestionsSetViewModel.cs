﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class CreateQuestionsSetViewModel
	{
        [Required(ErrorMessage = "Opis sprawdzanej umiejętności jest wymagany")]
        [MaxLength(2048)]
        [DisplayName("Opis umiejętności")]
        public string SkillDescription { get; set; }

        [Required(ErrorMessage = "Należy wybrać obszar zestawu pytań")]
        [DisplayName("Obszar zestawu pytań")]
        public byte? AreaId { get; set; }

        [Required(ErrorMessage = "Należy wybrać skalę trudności")]
        [DisplayName("Skala trudności")]
        public byte? DifficultyId { get; set; }

        [Required(ErrorMessage = "Oceny zestawu pytań są wymagane")]
        [DisplayName("Oceny zestawu pytań")]
        [MinLength(3, ErrorMessage = "Należy podać minimum 3 oceny")]
        [MaxLength(6, ErrorMessage = "Należy podać maksymalnie 6 ocen")]
        public IList<string> QuestionsSetRatings { get; set; }

        [DisplayName("Pytania")]
        public IEnumerable<QuestionViewModel>? Questions { get; set; }

        [DisplayName("Dołączone pliki - Karty pracy")]
        public IEnumerable<IFormFile>? AttachmentFiles { get; set; }
    }
}