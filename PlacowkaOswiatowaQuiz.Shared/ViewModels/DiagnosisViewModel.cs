using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
    public class DiagnosisViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Należy podać rok szkolny np.: 2022/2023")]
        [MaxLength(9)]
        [DisplayName("Rok szkolny:")]
        public string SchoolYear { get; set; }
        [DisplayName("Uczeń:")]
        public StudentViewModel Student { get; set; }
        [DisplayName("Prowadzący:")]
        public EmployeeViewModel Employee { get; set; }
        [DisplayName("Wyniki przeprowadzonej diagnozy")]
        public IList<ResultViewModel>? Results { get; set; }
        [DisplayName("Zestawy pytań")]
        public IList<int> QuestionsSetsIds { get; set; }
        [DisplayName("Skala trudności diagnozy:")]
        public DifficultyViewModel Difficulty { get; set; }
    }
}

