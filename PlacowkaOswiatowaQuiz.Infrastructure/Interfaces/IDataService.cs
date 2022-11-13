using PlacowkaOswiatowaQuiz.Data.Models;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Infrastructure.Interfaces
{
    public interface IDataService
    {
        Task<IEnumerable<EmployeeViewModel>> GetAllEmployees();
        Task<IEnumerable<UczenViewModel>> GetAllStudents();
        Task<IEnumerable<QuestionViewModel>> GetAllQuestions();
        Task<QuestionViewModel> GetQuestionById(int id);
        Task<Pytanie> AddQuestion(QuestionViewModel pytanieVM);
    }
}
