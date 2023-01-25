using System;
using System.ComponentModel;
using PlacowkaOswiatowaQuiz.Shared.Extensions;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class DiagnosisSummaryViewModel : BaseDiagnosisViewModel
    {
        [DisplayName("Zestawy pytań")]
        public IList<QuestionsSetViewModel>? QuestionsSets { get; set; }
    }
}