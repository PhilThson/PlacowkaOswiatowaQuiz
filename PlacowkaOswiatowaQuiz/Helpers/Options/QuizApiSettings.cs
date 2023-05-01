namespace PlacowkaOswiatowaQuiz.Helpers.Options
{
    public class QuizApiSettings
    {
        public User User { get; set; }
        public Data Data { get; set; }
    }

    public class User
    {
        public string Login { get; set; }
        public string ByEmail { get; set; }
        public string Data { get; set; }
        public string Logout { get; set; }
    }

    public class Data
    {
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
}
