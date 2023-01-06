using System;
namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class DiagnosisSummaryViewModel : DiagnosisViewModel
	{
        public IList<QuestionsSetViewModel> QuestionsSets { get; set; }
    }
}