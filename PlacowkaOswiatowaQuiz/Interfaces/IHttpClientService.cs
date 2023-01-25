using System;
namespace PlacowkaOswiatowaQuiz.Interfaces
{
	public interface IHttpClientService
	{
        Task<List<T>> GetAllItems<T>();
        Task<T> GetItemById<T>(object id);
        Task<T> GetItemByKey<T>(string key, string value);
        Task DeleteItemById<T>(object id);
        Task AddItem<T>(T item, string dict = null);
    }
}

