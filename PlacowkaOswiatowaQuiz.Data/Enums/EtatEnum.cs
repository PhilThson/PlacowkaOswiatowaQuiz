using System.ComponentModel;

namespace PlacowkaOswiatowaQuiz.Data.Enums
{
    public enum EtatEnum
    {
        [Description("Pracownik Administracyjny")]
        PracownikAdministracyjny,
        [Description("Pracownik Pedagogiczny")]
        PracownikPedagogiczny,
        [Description("Pracownik Obsługi")]
        PracownikObslugi
    }
}
