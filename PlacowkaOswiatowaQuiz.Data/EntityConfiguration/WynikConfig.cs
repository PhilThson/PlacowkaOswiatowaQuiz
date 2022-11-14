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
                .HasOne(w => w.OcenaZestawuPytan)
                .WithMany(o => o.OcenaZestawuPytanWyniki)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(w => w.Diagnoza)
                .WithMany(d => d.DiagnozaWyniki)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
