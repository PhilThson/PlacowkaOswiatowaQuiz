using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlacowkaOswiatowaQuiz.Data.Models;

namespace PlacowkaOswiatowaQuiz.Data.EntityConfiguration
{
    public class OdpowiedzConfig : IEntityTypeConfiguration<Odpowiedz>
    {
        public void Configure(EntityTypeBuilder<Odpowiedz> builder)
        {
            builder
                .HasOne(o => o.Pytanie)
                .WithMany(p => p.PytanieOdpowiedzi)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
