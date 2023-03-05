using System;
using PlacowkaOswiatowaQuiz.Shared.DTOs;

namespace PlacowkaOswiatowaQuiz.Interfaces
{
	public interface IUserService
	{
		Task<string> GetUserToken();
		Task<string> AuthenticateWithUserToken(string token);
        Task<IEnumerable<string>> Login(SimpleUserDto simpleUser);
		Task<SimpleUserDto> GetByEmail(string email);
		Task Register(CreateUserDto createUser);
		Task<string> GetData();
    }
}

