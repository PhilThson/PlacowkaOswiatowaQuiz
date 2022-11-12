using System;
namespace PlacowkaOswiatowaQuiz.Shared.ViewModels
{
	public class UczenViewModel
	{
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public DateTime? DataUrodzenia { get; set; }
        public string Pesel { get; set; }
        public string Oddzial { get; set; }
    }
}