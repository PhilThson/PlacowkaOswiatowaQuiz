using PlacowkaOswiatowaQuiz.Data.Models;
using Bogus;
using PlacowkaOswiatowaQuiz.Data.Enums;
using PlacowkaOswiatowaQuiz.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace PlacowkaOswiatowaQuiz.Data.Data
{
    public static class DbInitializer
    {
        #region Private fields
        private static List<Adres> Adresy { get; set; }
        private static List<Etat> Etaty { get; set; }
        private static List<ObszarZestawuPytan> ObszaryZestawowPytan { get; set; }
        private static List<Ocena> Oceny { get; set; }
        private static List<Oddzial> Oddzialy { get; set; }
        private static List<PracownicyAdresy> PracownicyAdresy { get; set; }
        private static List<Pracownik> Pracownicy { get; set; }
        private static List<Przedmiot> Przedmioty { get; set; }
        private static List<PrzedmiotyPracownicy> PrzedmiotyPracownicy { get; set; }
        private static List<Rola> Role { get; set; }
        private static List<SkalaTrudnosci> SkaleTrudnosci { get; set; }
        private static List<Stanowisko> Stanowiska { get; set; }
        private static List<Uczen> Uczniowie { get; set; }
        private static List<Uzytkownik> Uzytkownicy { get; set; }
        #endregion

        #region Seed
        public static void Seed(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<PlacowkaDbContext>();

                if (dbContext == null)
                {
                    throw new Exception("Nie udało się zainicjować DbContext'u");
                }

                dbContext.Database.EnsureCreated();
                if (dbContext.Database.GetAppliedMigrations().Count() < 1)
                {
                    if (dbContext.Pracownicy.Any() && dbContext.Uczniowie.Any())
                        return;

                    dbContext.Etaty.AddRange(EtatySeed());
                    dbContext.ObszaryZestawowPytan.AddRange(ObszaryZestawowPytanSeed());
                    dbContext.SkaleTrudnosci.AddRange(SkaleTrudnosciSeed());
                    dbContext.Przedmioty.AddRange(PrzedmiotySeed());
                    dbContext.Role.AddRange(RoleSeed());
                    dbContext.Stanowiska.AddRange(StanowiskaSeed());
                    dbContext.Adresy.AddRange(AdresySeed());
                    dbContext.SaveChanges();

                    dbContext.Pracownicy.AddRange(PracownicySeed(dbContext));
                    dbContext.SaveChanges();

                    dbContext.Oddzialy.AddRange(OddzialySeed());
                    dbContext.SaveChanges();
                    dbContext.Uczniowie.AddRange(UczniowieSeed(dbContext));
                    dbContext.SaveChanges();

                    dbContext.PracownicyAdresy.AddRange(PracownicyAdresySeed(dbContext));
                    dbContext.SaveChanges();

                    dbContext.PrzedmiotyPracownicy.AddRange(PrzedmiotyPracownicySeed(dbContext));
                    dbContext.SaveChanges();

                    dbContext.Oceny.AddRange(OcenySeed(dbContext));
                    dbContext.SaveChanges();

                    dbContext.Uzytkownicy.AddRange(UzytkownicySeed());

                    dbContext.SaveChanges();
                }
            }
        }
        #endregion


        #region Zasilanie encji słownikowych

        public static List<Etat> EtatySeed()
        {
            var etatFaker = new Faker<Etat>()
                .UseSeed(1122)
                //.RuleFor(e => e.Id, f => (byte)(f.IndexFaker + 1))
                .RuleFor(e => e.Nazwa, f => ((EtatEnum)f.IndexFaker).ToString())
                .RuleFor(e => e.CzyAktywny, f => true);

            Etaty = etatFaker.Generate(Enum.GetNames<EtatEnum>().Length);
            return Etaty;
        }

        public static List<ObszarZestawuPytan> ObszaryZestawowPytanSeed()
        {
            var obszarZestawuPytanFaker = new Faker<ObszarZestawuPytan>()
                .UseSeed(1212)
                //.RuleFor(e => e.Id, f => (byte)(f.IndexFaker + 1))
                .RuleFor(e => e.Nazwa, f => ((ObszarZestawuPytanEnum)f.IndexFaker).ToString())
                .RuleFor(e => e.Opis, f => ((ObszarZestawuPytanEnum)f.IndexFaker).GetDescription())
                .RuleFor(e => e.CzyAktywny, f => true);

            ObszaryZestawowPytan = obszarZestawuPytanFaker.Generate(
                Enum.GetNames<ObszarZestawuPytanEnum>().Length);
            return ObszaryZestawowPytan;
        }

        public static List<SkalaTrudnosci> SkaleTrudnosciSeed()
        {
            var skalaTrudnosciFaker = new Faker<SkalaTrudnosci>()
                .UseSeed(1221)
                //.RuleFor(e => e.Id, f => (byte)(f.IndexFaker + 1))
                .RuleFor(e => e.Nazwa, f => ((SkalaTrudnosciEnum)f.IndexFaker).ToString())
                .RuleFor(e => e.Opis, f => ((SkalaTrudnosciEnum)f.IndexFaker).GetDescription())
                .RuleFor(e => e.CzyAktywny, f => true);

            SkaleTrudnosci = skalaTrudnosciFaker.Generate(Enum.GetNames<SkalaTrudnosciEnum>().Length);
            return SkaleTrudnosci;
        }

        public static List<Przedmiot> PrzedmiotySeed()
        {
            var przedmiotFaker = new Faker<Przedmiot>()
                .UseSeed(5566)
                //.RuleFor(e => e.Id, f => (byte)(f.IndexFaker + 1))
                .RuleFor(e => e.Nazwa, f => ((PrzedmiotEnum)f.IndexFaker).ToString())
                .RuleFor(e => e.CzyAktywny, f => true);

            Przedmioty = przedmiotFaker.Generate(Enum.GetNames<PrzedmiotEnum>().Length);
            return Przedmioty;
        }

        public static List<Rola> RoleSeed()
        {
            var rolaFaker = new Faker<Rola>()
                .UseSeed(6677)
                //.RuleFor(e => e.Id, f => (byte)(f.IndexFaker + 1))
                .RuleFor(e => e.Nazwa, f => ((RolaEnum)f.IndexFaker).ToString())
                .RuleFor(e => e.CzyAktywny, f => true);

            Role = rolaFaker.Generate(Enum.GetNames<RolaEnum>().Length);
            return Role;
        }

        public static List<Stanowisko> StanowiskaSeed()
        {
            var stanowiskoFaker = new Faker<Stanowisko>()
                .UseSeed(7788)
                //.RuleFor(e => e.Id, f => (byte)(f.IndexFaker + 1))
                .RuleFor(e => e.Nazwa, f => ((StanowiskoEnum)f.IndexFaker).ToString())
                .RuleFor(e => e.CzyAktywny, f => true);

            Stanowiska = stanowiskoFaker.Generate(Enum.GetNames<StanowiskoEnum>().Length);
            return Stanowiska;
        }

        #endregion

        #region Zasilanie encji posiadających relacje klucza obcego
        public static List<Adres> AdresySeed()
        {
            var adresFaker = new Faker<Adres>()
                .UseSeed(9911)
                //.RuleFor(a => a.Id, f => f.IndexFaker + 1)
                .RuleFor(a => a.Panstwo, f => f.Address.Country())
                .RuleFor(a => a.Miejscowosc, f => f.Address.City())
                .RuleFor(a => a.Ulica, f => f.Address.StreetName())
                .RuleFor(a => a.NumerDomu, f => f.Random.Number(1, 200).ToString())
                .RuleFor(a => a.NumerMieszkania, f => f.Random.Number(1, 200).ToString())
                .RuleFor(a => a.KodPocztowy, f => f.Address.ZipCode("##-###"))
                .RuleFor(a => a.CzyAktywny, f => true);

            Adresy = adresFaker.Generate(200);
            return Adresy;
        }

        public static List<Pracownik> PracownicySeed(PlacowkaDbContext dbContext)
        {
            var stanowiskaIds = dbContext.Stanowiska.Select(s => s.Id).ToList();
            var etatyIds = dbContext.Etaty.Select(s => s.Id).ToList();

            var pracownikFaker = new Faker<Pracownik>()
                .UseSeed(1133)
                //.RuleFor(p => p.Id, f => f.IndexFaker + 1)
                .RuleFor(p => p.Imie, f => f.Person.FirstName)
                .RuleFor(p => p.Nazwisko, f => f.Person.LastName)
                .RuleFor(p => p.DataUrodzenia, f => f.Date.PastOffset(60, DateTime.Now.AddYears(-23)).Date)
                .RuleFor(p => p.Pesel, f => f.Random.ReplaceNumbers("##########"))
                .RuleFor(p => p.NrTelefonu, f => f.Phone.PhoneNumber("###-###-###"))
                .RuleFor(p => p.Email, f => f.Person.Email)
                .RuleFor(p => p.EtatId, f => f.PickRandom(etatyIds))
                .RuleFor(p => p.StanowiskoId, f => f.PickRandom(stanowiskaIds))
                .RuleFor(p => p.WymiarGodzinowy, f => f.Random.Int(15, 40))
                .RuleFor(p => p.Pensja, f => f.Finance.Amount(2800, 10000))
                .RuleFor(p => p.DniUrlopu, f => f.Random.Number(1, 100))
                .RuleFor(p => p.DataZatrudnienia, f => f.Date.PastOffset(10, DateTime.Now.AddDays(-1)).Date)
                .RuleFor(p => p.CzyAktywny, f => true);

            Pracownicy = pracownikFaker.Generate(50);
            return Pracownicy;
        }

        public static List<Oddzial> OddzialySeed()
        {
            int i = 1;
            var oddzialFaker = new Faker<Oddzial>()
                .UseSeed(3344)
                //.RuleFor(o => o.Id, f => (byte)(f.IndexFaker + 1))
                .RuleFor(o => o.Nazwa, f => ((OddzialEnum)f.IndexFaker).GetDescription())
                .RuleFor(o => o.Opis, f => ((OddzialEnum)f.IndexFaker).ToString())
                .RuleFor(o => o.PracownikId, f => i++)
                .RuleFor(o => o.CzyAktywny, f => true);

            Oddzialy = oddzialFaker.Generate(Enum.GetNames<OddzialEnum>().Length);
            return Oddzialy;
        }

        public static List<Uczen> UczniowieSeed(PlacowkaDbContext dbContext)
        {
            //var adresyIds = dbContext.Adresy.Select(s => s.Id).ToList();
            var pracownicyIds = dbContext.Pracownicy.Select(s => s.Id).ToList();
            var oddzialyIds = dbContext.Oddzialy.Select(s => s.Id).ToList();

            var uczenFaker = new Faker<Uczen>()
                .UseSeed(2244)
                //.RuleFor(u => u.Id, f => f.IndexFaker + 1)
                .RuleFor(u => u.Imie, f => f.Person.FirstName)
                .RuleFor(u => u.Nazwisko, f => f.Person.LastName)
                .RuleFor(u => u.DataUrodzenia, f => f.Date.PastOffset(15, DateTime.Now.AddYears(-3)).Date)
                .RuleFor(u => u.MiejsceUrodzenia, f => f.Person.Address.City)
                .RuleFor(u => u.Pesel, f => f.Random.ReplaceNumbers("##########"))
                //.RuleFor(u => u.AdresId, f => f.PickRandom(adresyIds))
                .RuleFor(u => u.WychowawcaId, f => f.PickRandom(pracownicyIds))
                .RuleFor(u => u.OddzialId, f => f.PickRandom(oddzialyIds));

            Uczniowie = uczenFaker.Generate(100);
            return Uczniowie;
        }

        #region Zasilanie tabel asocjacyjnych (wiele do wielu)
        public static List<PracownicyAdresy> PracownicyAdresySeed(PlacowkaDbContext dbContext)
        {
            var adresyIds = dbContext.Adresy.Select(s => s.Id).ToList();
            var pracownicyIds = dbContext.Pracownicy.Select(s => s.Id).ToList();

            var pracownicyAdresyFaker = new Faker<PracownicyAdresy>()
                .UseSeed(3355)
                .RuleFor(pa => pa.PracownikId, f => f.PickRandom(pracownicyIds))
                .RuleFor(pa => pa.AdresId, f => f.PickRandom(adresyIds));

            PracownicyAdresy = pracownicyAdresyFaker.Generate(50)
                .GroupBy(pa => new { pa.PracownikId, pa.AdresId }).Select(c => c.FirstOrDefault()).ToList();
            return PracownicyAdresy;
        }

        public static List<PrzedmiotyPracownicy> PrzedmiotyPracownicySeed(PlacowkaDbContext dbContext)
        {
            var pracownicyIds = dbContext.Pracownicy.Select(s => s.Id).ToList();
            var przedmiotyIds = dbContext.Przedmioty.Select(s => s.Id).ToList();

            var przedmiotyPracownicyFaker = new Faker<PrzedmiotyPracownicy>()
                .UseSeed(4466)
                .RuleFor(pp => pp.PracownikId, f => f.PickRandom(pracownicyIds))
                .RuleFor(pp => pp.PrzedmiotId, f => f.PickRandom(przedmiotyIds));

            PrzedmiotyPracownicy = przedmiotyPracownicyFaker.Generate(50)
                .GroupBy(pp => new { pp.PracownikId, pp.PrzedmiotId }).Select(c => c.FirstOrDefault()).ToList();
            return PrzedmiotyPracownicy;
        }
        #endregion

        public static List<Ocena> OcenySeed(PlacowkaDbContext dbContext)
        {
            var pracownicyIds = dbContext.Pracownicy.Select(s => s.Id).ToList();
            var przedmiotyIds = dbContext.Przedmioty.Select(s => s.Id).ToList();
            var uczniowieIds = dbContext.Uczniowie.Select(s => s.Id).ToList();

            var ocenaFaker = new Faker<Ocena>()
                .UseSeed(6688)
                //.RuleFor(o => o.Id, f => f.IndexFaker + 1)
                .RuleFor(o => o.PracownikId, f => f.PickRandom(pracownicyIds))
                .RuleFor(o => o.UczenId, f => f.PickRandom(uczniowieIds))
                .RuleFor(o => o.PrzedmiotId, f => f.PickRandom(przedmiotyIds))
                .RuleFor(o => o.WystawionaOcena, f => f.Random.Decimal(1, 6))
                .RuleFor(o => o.DataWystawienia, f => f.Date.Recent(100))
                .RuleFor(o => o.CzyAktywny, f => true);

            Oceny = ocenaFaker.Generate(100);
            return Oceny;
        }

        public static List<Uzytkownik> UzytkownicySeed()
        {
            Uzytkownicy = new List<Uzytkownik>
            {
                new Uzytkownik
                {
                    //Id = 1,
                    Email = "jan@kowalski.mail.com",
                    Imie = "Jan",
                    Nazwisko = "Kowalski",
                    RolaId = 2,
                    HashHasla = "AAA",
                    CzyAktywny = true
                },
                new Uzytkownik
                {
                    //Id = 2,
                    Email = "roman@nowak.mail.com",
                    Imie = "Roman",
                    Nazwisko = "Nowak",
                    RolaId = 3,
                    HashHasla = "BBB",
                    CzyAktywny = true
                },
                new Uzytkownik
                {
                    //Id = 3,
                    Email = "adam@wisniewski.mail.com",
                    Imie = "Adam",
                    Nazwisko = "Wiśniewski",
                    RolaId = 4,
                    HashHasla = "CCC",
                    CzyAktywny = true
                }
            };

            return Uzytkownicy;
        }

        #endregion
    }
}
