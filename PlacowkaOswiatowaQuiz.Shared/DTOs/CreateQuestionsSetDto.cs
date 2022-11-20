using System;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;
using System.ComponentModel;

namespace PlacowkaOswiatowaQuiz.Shared.DTOs
{
    public class CreateQuestionsSetDto
    {
        public string SkillDescription { get; set; }
        public byte AreaId { get; set; }
        public byte DifficultyId { get; set; }
        public IEnumerable<string> QuestionsSetRatings { get; set; }
        public IEnumerable<QuestionViewModel>? Questions { get; set; }
        public List<AttachmentFileViewModel>? AttachmentFiles { get; set; }
    }
}