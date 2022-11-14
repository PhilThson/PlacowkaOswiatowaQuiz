using System;
namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class QuestionsSetViewModel
	{
		public int  Id { get; set; }
		public string SkillDescription { get; set; }
		public string Area { get; set; }
		public string Difficulty { get; set; }
		public List<string> QuestionsSetRatings { get; set; }
		public List<QuestionViewModel> Questions { get; set; }
		public AttachmentFileViewModel AttachmentFile { get; set; }
    }
}