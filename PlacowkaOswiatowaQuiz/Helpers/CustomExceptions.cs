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

    public class DataValidationException : Exception
    {
        public DataValidationException()
            : base("Niepoprawnie wprowadzone dane")
        {

        }

        public DataValidationException(string message) : base(message)
        {

        }
    }
}

