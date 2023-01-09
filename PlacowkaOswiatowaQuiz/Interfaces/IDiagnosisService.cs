using PlacowkaOswiatowaQuiz.Shared.DTOs;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Interfaces
{
    public interface IDiagnosisService
	{
        Task<List<DiagnosisViewModel>> GetAllDiagnosis();
        Task<DiagnosisViewModel> GetDiagnosisById(int id);
        Task<DiagnosisViewModel> CreateDiagnosis(
            CreateDiagnosisViewModel diagnosisVM);
        Task CreateResult(ResultViewModel resultVM);
        Task<ResultViewModel> GetResultById(int id);
        Task<ResultViewModel> GetResultByDiagnosisQuestionsSetIds(
            int diagnosisId, int questionsSetId);
        Task<BaseReportDto> CreateDiagnosisReport(int diagnosisId);
        Task<ReportDto> GetDiagnosisReportById(int reportId);
    }
}

