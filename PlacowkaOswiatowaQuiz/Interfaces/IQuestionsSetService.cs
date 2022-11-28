using System;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Interfaces
{
	public interface IQuestionsSetService
	{
        Task<List<QuestionsSetViewModel>> GetAllQuestionsSets();
        Task<QuestionsSetViewModel> GetQuestionsSetById(int id);
    }
}

