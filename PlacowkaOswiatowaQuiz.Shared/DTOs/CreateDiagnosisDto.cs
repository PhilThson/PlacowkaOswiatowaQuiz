using System;
namespace PlacowkaOswiatowaQuiz.Shared.DTOs
{
    public class CreateDiagnosisDto
    {
        public int EmployeeId { get; set; }
        public int StudentId { get; set; }
        public string SchoolYear { get; set; }
        public byte DifficultyId { get; set; }
    }
}