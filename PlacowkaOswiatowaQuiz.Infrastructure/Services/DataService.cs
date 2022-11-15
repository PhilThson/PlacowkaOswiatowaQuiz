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

        public async Task<IEnumerable<StudentViewModel>> GetAllStudents() =>
            await _dbContext.Uczniowie.Select(u => new StudentViewModel
            {
                Id = u.Id,
                FirstName = u.Imie,
                LastName = u.Nazwisko,
                DateOfBirth = u.DataUrodzenia,
                PersonalNumber = u.Pesel,
                Branch = u.Oddzial.Nazwa
            })
            .ToListAsync();

        public async Task<IEnumerable<QuestionsSetViewModel>> GetAllQuestionsSets() =>
            await _dbContext.ZestawyPytan.Select(z => new QuestionsSetViewModel
            {
                Id = z.Id,
                SkillDescription = z.OpisUmiejetnosci,
                Area = z.ObszarZestawuPytan.Nazwa,
                Difficulty = z.SkalaTrudnosci.Nazwa,
                Questions = new List<QuestionViewModel>
                    (
                        z.ZestawPytanPytania.Select(p => new QuestionViewModel
                        {
                            Id = p.Id,
                            Content = p.Tresc,
                            Description = p.Opis,
                            QuestionsSetId = p.ZestawPytanId
                        })
                    ),
                QuestionsSetRatings = z.ZestawPytanOceny
                        .Select(o => o.OpisOceny).ToArray(),
                AttachmentFile = new AttachmentFileViewModel
                {
                    Id = z.KartaPracy.Id,
                    Name = z.KartaPracy.Nazwa,
                    ContentType = z.KartaPracy.RodzajZawartosci,
                    Size = z.KartaPracy.Rozmiar
                }
            })
            .ToListAsync();

        public async Task<QuestionsSetViewModel> GetQuestionsSetById(int id) =>
            await _dbContext.ZestawyPytan
            .Where(z => z.Id == id)
            .Select(z => new QuestionsSetViewModel
            {
                Id = z.Id,
                SkillDescription = z.OpisUmiejetnosci,
                Area = z.ObszarZestawuPytan.Nazwa,
                Difficulty = z.SkalaTrudnosci.Nazwa,
                Questions = new List<QuestionViewModel>
                (
                    z.ZestawPytanPytania.Select(p => new QuestionViewModel
                    {
                        Id = p.Id,
                        Content = p.Tresc,
                        Description = p.Opis,
                        QuestionsSetId = p.ZestawPytanId
                    })
                ),
                QuestionsSetRatings = z.ZestawPytanOceny
                    .Select(o => o.OpisOceny).ToArray(),
                AttachmentFile = new AttachmentFileViewModel
                {
                    Id = z.KartaPracy.Id,
                    Name = z.KartaPracy.Nazwa,
                    ContentType = z.KartaPracy.RodzajZawartosci,
                    Size = z.KartaPracy.Rozmiar
                }
            })
            .FirstOrDefaultAsync()
            ?? throw new DataNotFoundException();

        public async Task<IEnumerable<QuestionViewModel>> GetAllQuestions() =>
            await _dbContext.Pytania
            .Select(p => new QuestionViewModel
            {
                Id = p.Id,
                Content = p.Tresc,
                Description = p.Opis,
                QuestionsSetId = p.ZestawPytanId
            })
            .ToListAsync();

        public async Task<QuestionViewModel> GetQuestionById(int id) =>
            await _dbContext.Pytania
            .Where(p => p.Id == id)
            .Select(p => new QuestionViewModel
            {
                Id = p.Id,
                Content = p.Tresc,
                Description = p.Opis,
                QuestionsSetId = p.ZestawPytanId
            })
            .FirstOrDefaultAsync()
            ?? throw new DataNotFoundException();

        public async Task<AttachmentFileViewModel> GetAttachmentById(int id) =>
            await _dbContext.KartyPracy
            .Where(k => k.Id == id)
            .Select(k => new AttachmentFileViewModel
            {
                Id = k.Id,
                Name = k.Nazwa,
                Content = k.Zawartosc,
                ContentType = k.RodzajZawartosci,
                Size = k.Rozmiar
            })
            .FirstOrDefaultAsync()
            ?? throw new DataNotFoundException();


        public async Task<Pytanie> AddQuestion(QuestionViewModel questionVM)
        {
            var question = new Pytanie
            {
                Tresc = questionVM.Content,
                Opis = questionVM.Description,
                ZestawPytanId = questionVM.QuestionsSetId.Value
            };

            await _dbContext.Pytania.AddAsync(question);
            await _dbContext.SaveChangesAsync();

            return question;
        }

        public async Task<ZestawPytan> AddQuestionsSet(
            QuestionsSetViewModel questionsSetVM)
        {
            var questionsSet = new ZestawPytan
            {
                ObszarZestawuPytanId = await GetObjectId<ObszarZestawuPytan, byte>
                    (questionsSetVM.Area),
                SkalaTrudnosciId = await GetObjectId<SkalaTrudnosci, byte>
                    (questionsSetVM.Difficulty)
            };

            return questionsSet;
        }

        private async Task<TKey> GetObjectId<T, TKey>(string name)
            where T : BaseDictionaryEntity<TKey> =>
            await _dbContext.Set<T>()
                .Where(e => e.Nazwa == name)
                .Select(e => e.Id)
                .FirstOrDefaultAsync() ?? throw new DataNotFoundException();
    }
}
