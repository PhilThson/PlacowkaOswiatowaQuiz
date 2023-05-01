using System;
namespace PlacowkaOswiatowaQuiz.Interfaces
{
	public interface IHttpClientService
	{
        Task<List<T>> GetAllItems<T>(params (string, object)[] queryParams);

        Task<T> GetItemById<T>(object? id, params (string, object)[] queryParams);

        Task DeleteItemById<T>(object id);

        Task<object> AddItem<T>(T item, params (string, object)[] queryParams);

        Task UpdateItem<T>(T item, params (string, object)[] queryParams);

        Task<string> UpdateItemProperty<T>(object itemId,
            KeyValuePair<string, string> propertyValue);
    }
}

