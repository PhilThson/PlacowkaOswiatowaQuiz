using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlacowkaOswiatowaQuiz.Data.Models;

namespace PlacowkaOswiatowaQuiz.Data.EntityConfiguration
{
    public class PracownikConfig : IEntityTypeConfiguration<Pracownik>
    {
        public void Configure(EntityTypeBuilder<Pracownik> builder)
        {
            builder
                .Property(p => p.Pensja)
                .HasColumnType("money")
                .HasPrecision(7, 2)
                .IsRequired(true);
        }
    }
}
