using System;
namespace PlacowkaOswiatowaQuiz.Interfaces
{
	public interface IUserService
	{
		Task<string> GetUserToken();
		Task<string> AuthenticateWithUserToken(string token);

    }
}

