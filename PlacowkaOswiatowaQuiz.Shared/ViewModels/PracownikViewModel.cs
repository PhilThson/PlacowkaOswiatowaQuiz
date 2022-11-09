namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
    public class PracownikViewModel
    {
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public DateTime? DataUrodzenia { get; set; }
        public string Pesel { get; set; }
        public decimal Pensja { get; set; }
        public string? Email { get; set; }
        public string Etat { get; set; }
        public string Stanowisko { get; set; }
        public DateTime DataZatrudnienia { get; set; }
    }
}
