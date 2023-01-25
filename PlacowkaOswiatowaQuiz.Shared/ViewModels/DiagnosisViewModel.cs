using System.ComponentModel;
using PlacowkaOswiatowaQuiz.Shared.Extensions;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
    public class DiagnosisViewModel : BaseDiagnosisViewModel
    {
        [DisplayName("Identyfikatory zestawów pytań")]
        public IList<int>? QuestionsSetsIds { get; set; }
        public bool IsForEdit { get; set; }

        public static explicit operator DiagnosisSummaryViewModel(
            DiagnosisViewModel diagnosisVM)
        {
            var diagnosisSummary = new DiagnosisSummaryViewModel();
            diagnosisSummary.Map(diagnosisVM);
            return diagnosisSummary;
        }
    }
}