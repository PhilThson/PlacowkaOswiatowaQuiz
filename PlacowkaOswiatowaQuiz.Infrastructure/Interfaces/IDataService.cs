using PlacowkaOswiatowaQuiz.Data.Models;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Infrastructure.Interfaces
{
    public interface IDataService
    {
        Task<IEnumerable<EmployeeViewModel>> GetAllEmployees();
        Task<IEnumerable<StudentViewModel>> GetAllStudents();
        Task<IEnumerable<QuestionsSetViewModel>> GetAllQuestionsSets();
        Task<QuestionsSetViewModel> GetQuestionsSetById(int id);
        Task<IEnumerable<QuestionViewModel>> GetAllQuestions();
        Task<QuestionViewModel> GetQuestionById(int id);
        Task<AttachmentFileViewModel> GetAttachmentById(int id);
        Task<Pytanie> AddQuestion(QuestionViewModel questionVM);
        Task<ZestawPytan> AddQuestionsSet(
            QuestionsSetViewModel questionsSetVM);
    }
}
