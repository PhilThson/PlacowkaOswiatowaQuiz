using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PlacowkaOswiatowaQuiz.Shared.DTOs;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class CreateDiagnosisViewModel
	{
        [Required(ErrorMessage = "Należy podać pełną nazwę placówki oświatowej")]
        [DisplayName("Nazwa placówki oświatowej")]
        [StringLength(512)]
        public string Institution { get; set; }
        [Required(ErrorMessage = "Należy podać rok szkolny np.: 2022/2023")]
        [StringLength(9)]
        [DisplayName("Rok szkolny")]
        public string SchoolYear { get; set; }
        [Required(ErrorMessage = "Należy podać pełną nazwę PPP")]
        [StringLength(512)]
        [DisplayName("PPP:")]
        public string CounselingCenter { get; set; }
        [Required(ErrorMessage = "Należy wybrać ucznia")]
        [DisplayName("Uczeń")]
        public int? StudentId { get; set; }
        [Required(ErrorMessage = "Należy wybrać prowadzącego")]
        [DisplayName("Prowadzący")]
        public int? EmployeeId { get; set; }
        [Required(ErrorMessage = "Należy wskazać skalę trudności")]
        [DisplayName("Skala trudności")]
        public byte? DifficultyId { get; set; }

        public static explicit operator CreateDiagnosisDto(CreateDiagnosisViewModel diagnosisVM) =>
            new CreateDiagnosisDto
            {
                Institution = diagnosisVM.Institution,
                SchoolYear = diagnosisVM.SchoolYear,
                CounselingCenter = diagnosisVM.CounselingCenter,
                StudentId = diagnosisVM.StudentId.Value,
                EmployeeId = diagnosisVM.EmployeeId.Value,
                DifficultyId = diagnosisVM.DifficultyId.Value
            };
    }
}