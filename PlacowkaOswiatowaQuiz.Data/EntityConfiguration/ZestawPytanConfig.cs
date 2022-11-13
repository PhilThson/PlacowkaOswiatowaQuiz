using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlacowkaOswiatowaQuiz.Data.Models;

namespace PlacowkaOswiatowaQuiz.Data.EntityConfiguration
{
	public class ZestawPytanConfig : IEntityTypeConfiguration<ZestawPytan>
	{
        public void Configure(EntityTypeBuilder<ZestawPytan> builder)
        {
            builder
                .HasOne(z => z.ObszarZestawuPytan)
                .WithMany(o => o.ObszarZestawuPytanZestawyPytan)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(z => z.SkalaTrudnosci)
                .WithMany(s => s.SkalaTrudnosciZestawyPytan)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

