using Microsoft.EntityFrameworkCore;
using PlacowkaOswiatowaQuiz.Data.Models;
using System.Reflection;

namespace PlacowkaOswiatowaQuiz.Data.Data
{
    public class PlacowkaDbContext : DbContext
    {
        public PlacowkaDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Adres> Adresy { get; set; }
        public DbSet<Diagnoza> Diagnozy { get; set; }
        public DbSet<Etat> Etaty { get; set; }
        public DbSet<Migawka> Migawki { get; set; }
        public DbSet<ObszarZestawuPytan> ObszaryZestawowPytan { get; set; }
        public DbSet<Ocena> Oceny { get; set; }
        public DbSet<Oddzial> Oddzialy { get; set; }
        public DbSet<OcenaZestawuPytan> OcenyZestawowPytan { get; set; }
        public DbSet<PracownicyAdresy> PracownicyAdresy { get; set; }
        public DbSet<Pracownik> Pracownicy { get; set; }
        public DbSet<Przedmiot> Przedmioty { get; set; }
        public DbSet<PrzedmiotyPracownicy> PrzedmiotyPracownicy { get; set; }
        public DbSet<Pytanie> Pytania { get; set; }
        public DbSet<Rola> Role { get; set; }
        public DbSet<SkalaTrudnosci> SkaleTrudnosci { get; set; }
        public DbSet<Stanowisko> Stanowiska { get; set; }
        public DbSet<Uczen> Uczniowie { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<Wynik> Wyniki { get; set; }
        public DbSet<ZestawPytan> ZestawyPytan { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
