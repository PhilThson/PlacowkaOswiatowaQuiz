using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlacowkaOswiatowaQuiz.Data.Models;

namespace PlacowkaOswiatowaQuiz.Data.EntityConfiguration
{
    public class OcenaConfig : IEntityTypeConfiguration<Ocena>
    {
        public void Configure(EntityTypeBuilder<Ocena> builder)
        {
            builder
                .Property(o => o.DataWystawienia)
                .HasColumnType("datetime")
                .HasComputedColumnSql("getdate()");

            builder
                .Property(o => o.DataPoprawienia)
                .HasColumnType("datetime")
                .IsRequired(false);

            //Decimal(5,4) - razem 5 cyfr, z czego 4 po przecinku
            builder
                .Property(o => o.WystawionaOcena)
                .HasColumnType("decimal")
                .HasPrecision(3, 2)
                .IsRequired(true);

            builder
                .Property(o => o.PoprzedniaOcena)
                .HasColumnType("decimal")
                .HasPrecision(3, 2)
                .IsRequired(false);
        }
    }
}
