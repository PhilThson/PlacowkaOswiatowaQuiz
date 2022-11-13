using System;
namespace PlacowkaOswiatowaQuiz.Infrastructure
{
	public class DataNotFoundException : Exception
	{
		public DataNotFoundException()
			: base("Nie znaleziono danych dla wskazanych parametrów")
		{}

		public DataNotFoundException(string message)
			: base(message)
		{}
	}
}

