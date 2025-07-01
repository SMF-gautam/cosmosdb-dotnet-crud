using CosmosDB.Entities;

namespace CosmosDB.IService
{
    public interface ICosmosDbService
    {
        Task<Item> GetItemAsync(string id);
        Task<(IEnumerable<Item>, string?)> GetItemsAsync(string queryString, string? continuationToken, int pageSize = 10);
        Task AddItemAsync(Item newItem);
        Task UpdateItemAsync(string id, Item updatedItem);
        Task DeleteItemAsync(string id);
    }
}