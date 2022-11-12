using Microsoft.EntityFrameworkCore;
using PlacowkaOswiatowaQuiz.Data.Data;
using PlacowkaOswiatowaQuiz.Infrastructure.Interfaces;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Infrastructure.Services
{
    public class DataService : IDataService
    {
        private readonly PlacowkaDbContext _dbContext;

        public DataService(PlacowkaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PracownikViewModel>> GetAllEmployees() =>
            await _dbContext.Pracownicy.Select(p => new PracownikViewModel
            {
                Id = p.Id,
                Imie = p.Imie,
                Nazwisko = p.Nazwisko,
                DataUrodzenia = p.DataUrodzenia,
                Pesel = p.Pesel,
                Pensja = p.Pensja,
                Email = p.Email,
                Etat = p.Etat.Nazwa,
                Stanowisko = p.Stanowisko.Nazwa,
                DataZatrudnienia = p.DataZatrudnienia
            })
            .ToListAsync();

        public async Task<IEnumerable<UczenViewModel>> GetAllStudents() =>
            await _dbContext.Uczniowie.Select(u => new UczenViewModel
            {
                Id = u.Id,
                Imie = u.Imie,
                Nazwisko = u.Nazwisko,
                DataUrodzenia = u.DataUrodzenia,
                Pesel = u.Pesel,
                Oddzial = u.Oddzial.Nazwa
            })
            .ToListAsync();
    }
}
