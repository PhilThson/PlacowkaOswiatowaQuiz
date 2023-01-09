using System.ComponentModel;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
    public class DiagnosisViewModel : BaseDiagnosisViewModel
    {
        [DisplayName("Identyfikatory zestawów pytań")]
        public IList<int> QuestionsSetsIds { get; set; }
    }
}