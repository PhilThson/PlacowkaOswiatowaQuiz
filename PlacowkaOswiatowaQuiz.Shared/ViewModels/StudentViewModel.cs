using System;
using PlacowkaOswiatowaQuiz.Shared.DTOs;

namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class StudentViewModel
	{
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PersonalNumber { get; set; }
        public string DisabilityCert { get; set; }
        public string PlaceOfBirth { get; set; }
        public BranchDto? Branch { get; set; }
    }
}