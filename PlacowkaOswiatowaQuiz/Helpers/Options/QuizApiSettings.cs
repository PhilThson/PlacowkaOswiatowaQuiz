namespace PlacowkaOswiatowaQuiz.Helpers.Options
{
    public class QuizApiSettings
    {
        public UserController User { get; set; }
        public string MainController { get; set; }
        public string Employees { get; set; }
        public string Students { get; set; }
        public string Questions { get; set; }
        public string QuestionsSets { get; set; }
        public string Areas { get; set; }
        public string Difficulties { get; set; }
        public string Ratings { get; set; }
        public string Attachments { get; set; }
        public string Diagnosis { get; set; }
        public string Results { get; set; }
        public string QuestionsSetsAsked { get; set; }
        public string Reports { get; set; }
    }

    public class UserController
    {
        public string Login { get; set; }
        public string ByEmail { get; set; }
        public string Data { get; set; }
    }
}
