using System;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Interfaces
{
	public interface IDiagnosisService
	{
        Task<List<DiagnosisViewModel>> GetAllDiagnosis();
        Task<DiagnosisViewModel> GetDiagnosisById(int id);
        Task CreateDiagnosis(CreateDiagnosisViewModel diagnosisVM);
    }
}

