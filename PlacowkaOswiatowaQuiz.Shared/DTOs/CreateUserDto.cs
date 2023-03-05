using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlacowkaOswiatowaQuiz.Shared.DTOs
{
	public class CreateUserDto
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public byte? RoleId { get; set; }
    }
}

