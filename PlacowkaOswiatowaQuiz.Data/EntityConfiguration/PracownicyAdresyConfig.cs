using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlacowkaOswiatowaQuiz.Data.Models;

namespace PlacowkaOswiatowaQuiz.Data.EntityConfiguration
{
    public class PracownicyAdresyConfig : IEntityTypeConfiguration<PracownicyAdresy>
    {
        public void Configure(EntityTypeBuilder<PracownicyAdresy> builder)
        {
            builder.HasKey(pa => new { pa.PracownikId, pa.AdresId });
        }
    }
}
