using System;
using PlacowkaOswiatowaQuiz.Shared.ViewModels;

namespace PlacowkaOswiatowaQuiz.Interfaces
{
	public interface IQuestionsSetService
	{
        Task<List<QuestionsSetViewModel>> GetAllQuestionsSets(
            byte? difficultyId = null);
        Task<QuestionsSetViewModel> GetQuestionsSetById(int id);
        Task<List<RatingViewModel>> GetRatingsByQuestionsSetId(int id);
    }
}

