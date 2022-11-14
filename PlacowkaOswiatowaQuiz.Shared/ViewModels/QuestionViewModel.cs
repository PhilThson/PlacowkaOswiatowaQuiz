using System;
namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class QuestionViewModel
	{
		public int? Id { get; set; }
		public string Content { get; set; }
		public string Description { get; set; }
		public int? QuestionsSetId { get; set; }
	}
}