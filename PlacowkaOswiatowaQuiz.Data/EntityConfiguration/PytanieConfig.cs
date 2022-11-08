using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlacowkaOswiatowaQuiz.Data.Models;

namespace PlacowkaOswiatowaQuiz.Data.EntityConfiguration
{
    public class PytanieConfig : IEntityTypeConfiguration<Pytanie>
    {
        public void Configure(EntityTypeBuilder<Pytanie> builder)
        {
            builder
                .HasOne(p => p.ObszarPytania)
                .WithMany(o => o.ObszarPytaniePytania)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.SkalaTrudnosci)
                .WithMany(s => s.SkalaTrudnosciPytania)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
