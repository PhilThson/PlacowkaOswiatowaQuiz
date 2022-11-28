﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
    public class DiagnosisViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Należy podać rok szkolny np.: 2022/2023")]
        [MaxLength(9)]
        [DisplayName("Rok szkolny")]
        public string SchoolYear { get; set; }
        [DisplayName("Uczeń")]
        public StudentViewModel Student { get; set; }
        [DisplayName("Prowadzący")]
        public EmployeeViewModel Employee { get; set; }
        [DisplayName("Wyniki przeprowadzonej diagnozy")]
        public virtual IList<ResultViewModel>? Results { get; set; }
    }
}

