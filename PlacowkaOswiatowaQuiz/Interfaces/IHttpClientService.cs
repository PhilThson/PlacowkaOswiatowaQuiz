using System;
namespace PlacowkaOswiatowaQuiz.Interfaces
{
	public interface IHttpClientService
	{
        Task<List<T>> GetAllItems<T>(params (string, string)[] queryParams);

        Task<T> GetItemById<T>(object? id, params (string, string)[] queryParams);

        Task DeleteItemById<T>(object id);

        Task<object> AddItem<T>(T item, params (string, string)[] queryParams);

        Task UpdateItem<T>(T item, params (string, string)[] queryParams);

        Task<string> UpdateItemProperty<T>(object itemId,
            KeyValuePair<string, string> propertyValue);
    }
}

