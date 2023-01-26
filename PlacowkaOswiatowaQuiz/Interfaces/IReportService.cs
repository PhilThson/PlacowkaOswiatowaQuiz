using PlacowkaOswiatowaQuiz.Shared.DTOs;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Interfaces
{
    public interface IReportService
	{
        Task<BaseReportDto> CreateDiagnosisReport(int diagnosisId);
    }
}

