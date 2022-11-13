using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlacowkaOswiatowaQuiz.Data.Models;

namespace PlacowkaOswiatowaQuiz.Data.EntityConfiguration
{
    public class OcenaZestawuPytanConfig : IEntityTypeConfiguration<OcenaZestawuPytan>
    {
        public void Configure(EntityTypeBuilder<OcenaZestawuPytan> builder)
        {
            builder
                .HasOne(o => o.ZestawPytan)
                .WithMany(p => p.ZestawPytanOcena)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
