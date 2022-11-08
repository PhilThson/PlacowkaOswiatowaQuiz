using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlacowkaOswiatowaQuiz.Data.Models;

namespace PlacowkaOswiatowaQuiz.Data.EntityConfiguration
{
    public class WynikConfig : IEntityTypeConfiguration<Wynik>
    {
        public void Configure(EntityTypeBuilder<Wynik> builder)
        {
            builder
                .HasOne(w => w.Uczen)
                .WithMany(u => u.UczenWyniki)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(w => w.Pracownik)
                .WithMany(p => p.PracownikWyniki)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(w => w.Odpowiedz)
                .WithMany(o => o.OdpowiedzWyniki)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(w => w.Pytanie)
                .WithMany(p => p.PytanieWyniki)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
