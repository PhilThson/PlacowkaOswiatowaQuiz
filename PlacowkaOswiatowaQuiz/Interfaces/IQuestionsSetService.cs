using System;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Interfaces
{
	public interface IQuestionsSetService
	{
        Task<List<QuestionsSetViewModel>> GetAllQuestionsSets(
            byte? difficultyId = null);
        Task<List<QuestionsSetViewModel>> GetQuestionsSetsByIds(List<int> ids);
        Task<QuestionsSetViewModel> GetQuestionsSetById(int id);
        Task<List<RatingViewModel>> GetRatingsByQuestionsSetId(int id);
    }
}

