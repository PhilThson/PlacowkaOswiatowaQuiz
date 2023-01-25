using System;
namespace PlacowkaOswiatowaQuiz.Shared.DTOs
{
    public class CreateDiagnosisDto
    {
        public string Institution { get; set; }
        public string SchoolYear { get; set; }
        public string CounselingCenter { get; set; }
        public int StudentId { get; set; }
        public int EmployeeId { get; set; }
        public byte DifficultyId { get; set; }
    }
}