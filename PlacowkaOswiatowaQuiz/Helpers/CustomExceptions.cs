using System;
namespace PlacowkaOswiatowaQuiz.Helpers
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException()
            : base("Nie znaleziono danych dla wskazanych parametrów")
        {

        }

        public DataNotFoundException(string message) : base(message)
        {

        }
    }
}

