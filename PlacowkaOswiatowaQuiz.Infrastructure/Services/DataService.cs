using Microsoft.EntityFrameworkCore;
using PlacowkaOswiatowaQuiz.Data.Data;
using PlacowkaOswiatowaQuiz.Data.Models;
using PlacowkaOswiatowaQuiz.Data.Models.Base;
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

        public async Task<IEnumerable<EmployeeViewModel>> GetAllEmployees() =>
            await _dbContext.Pracownicy.Select(p => new EmployeeViewModel
                {
                    Id = p.Id,
                    FirstName = p.Imie,
                    LastName = p.Nazwisko,
                    DateOfBirth = p.DataUrodzenia,
                    PersonalNumber = p.Pesel,
                    Salary = p.Pensja,
                    Email = p.Email,
                    Job = p.Etat.Nazwa,
                    Position = p.Stanowisko.Nazwa,
                    DateOfEmployment = p.DataZatrudnienia
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

        public async Task<IEnumerable<QuestionViewModel>> GetAllQuestions() =>
            await _dbContext.Pytania.Select(p => new QuestionViewModel
            {
                    Id = p.Id,
                    Content = p.Tresc,
                    Area = p.ObszarPytania.Nazwa,
                    Difficulty = p.SkalaTrudnosci.Nazwa
                })
                .ToListAsync();

        public async Task<QuestionViewModel> GetQuestionById(int id) =>
            await _dbContext.Pytania
                .Where(p => p.Id == id)
                .Select(p => new QuestionViewModel
                {
                    Id = p.Id,
                    Content = p.Tresc,
                    Area = p.ObszarPytania.Nazwa,
                    Difficulty = p.SkalaTrudnosci.Nazwa
                })
                .FirstOrDefaultAsync()
            ?? throw new DataNotFoundException();

        public async Task<Pytanie> AddQuestion(QuestionViewModel pytanieVM)
        {
            var question = new Pytanie
            {
                Tresc = pytanieVM.Content,
                ObszarPytaniaId = await GetObjectId<ObszarPytania, byte>(pytanieVM.Area),
                SkalaTrudnosciId = await GetObjectId<SkalaTrudnosci, byte>(pytanieVM.Difficulty)
            };

            await _dbContext.Pytania.AddAsync(question);
            await _dbContext.SaveChangesAsync();

            return question;
        }

        private async Task<TKey> GetObjectId<T, TKey>(string name)
            where T : BaseDictionaryEntity<TKey> =>
            await _dbContext.Set<T>()
                .Where(e => e.Nazwa == name)
                .Select(e => e.Id)
                .FirstOrDefaultAsync() ?? throw new DataNotFoundException();
    }
}
