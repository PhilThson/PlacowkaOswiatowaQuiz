using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Infrastructure.Interfaces
{
    public interface IDataService
    {
        Task<IEnumerable<PracownikViewModel>> GetAllEmployees();
    }
}
