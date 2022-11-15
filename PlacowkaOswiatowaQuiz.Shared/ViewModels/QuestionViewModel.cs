using System;
using System.ComponentModel;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class QuestionViewModel
	{
		public int? Id { get; set; }
        [DisplayName("Treść")]
        public string Content { get; set; }
        [DisplayName("Opis")]
        public string Description { get; set; }
        [DisplayName("Id zestawu pytań")]
        public int? QuestionsSetId { get; set; }
	}
}