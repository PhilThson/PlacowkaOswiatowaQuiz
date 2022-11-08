using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlacowkaOswiatowaQuiz.Data.Models;

namespace PlacowkaOswiatowaQuiz.Data.EntityConfiguration
{
    public class UczniowieConfig : IEntityTypeConfiguration<Uczen>
    {
        public void Configure(EntityTypeBuilder<Uczen> builder)
        {
            builder
                .HasOne(u => u.Wychowawca)
                .WithMany(p => p.PracownikUczniowie)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(u => u.Oddzial)
                .WithMany(o => o.OddzialUczniowie)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(u => u.UczenOceny)
                .WithOne(o => o.Uczen)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
