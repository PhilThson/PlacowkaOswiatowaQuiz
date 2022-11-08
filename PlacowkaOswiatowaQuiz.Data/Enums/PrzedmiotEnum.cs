using System.ComponentModel;

namespace PlacowkaOswiatowaQuiz.Data.Enums
{
    public enum PrzedmiotEnum
    {
        [Description("Edukacja Wczesnoszkolna")]
        EdukacjaWczesnoszkolna,
        Informatyka,
        Matematyka,
        [Description("Język Polski")]
        JęzykPolski,
        [Description("Język Angielski")]
        JęzykAngielski,
        [Description("Język Niemiecki")]
        JęzykNiemiecki,
        Fizyka,
        Biologia,
        Chemia,
        Historia,
        Geografia,
        Muzyka,
        Plastyka,
        Technika,
        Przyroda,
        [Description("Wiedza O Społeczeństwie")]
        WiedzaOSpołeczeństwie,
        [Description("Wychowanie Fizyczne")]
        WychowanieFizyczne
    }
}
