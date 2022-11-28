using System;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Shared.DTOs
{
    public class CreateResultDto
    {
        public int DiagnosisId { get; set; }
        public int RatingId { get; set; }
        [Range(1, 6)]
        public byte RatingLevel { get; set; }
        [MaxLength(2048)]
        public string? Notes { get; set; }
    }
}

