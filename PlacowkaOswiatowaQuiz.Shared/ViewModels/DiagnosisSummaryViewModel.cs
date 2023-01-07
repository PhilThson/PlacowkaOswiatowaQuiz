using System;
using System.ComponentModel;
using PlacowkaOswiatowaQuiz.Shared.Extensions;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class DiagnosisSummaryViewModel : BaseDiagnosisViewModel
    {
        [DisplayName("Zestawy pytań")]
        public IList<QuestionsSetViewModel> QuestionsSets { get; set; }

        //public static explicit operator DiagnosisSummaryViewModel(
        //    DiagnosisViewModel diagnosisViewModel)
        //{
        //    var diagnosisSummary = new DiagnosisSummaryViewModel();
        //    diagnosisSummary.CopyPropertiesExtension(diagnosisViewModel);
        //    return diagnosisSummary;
        //}
    }
}