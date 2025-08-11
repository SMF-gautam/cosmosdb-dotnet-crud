using CosmosDB.Entities;
using CosmosDB.IService;
using Microsoft.Azure.Cosmos;
using System.Net;

namespace CosmosDB.Service;

public class CosmosDbService(CosmosClient client, string databaseName, string containerName, ILogger<CosmosDbService> _logger) : ICosmosDbService
{
    private readonly Container _container = client.GetContainer(databaseName, containerName);

    public async Task<Item> GetItemAsync(string id)
    {
        try
        {
            ItemResponse<Item> response =
                await _container.ReadItemAsync<Item>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogWarning(ex, "Item not found: {Id}", id);
            return null!;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
        {
            _logger.LogWarning(ex, "Rate limited. Retry after: {RetryAfter}", ex.RetryAfter);
            throw;
        }
        catch (CosmosException ex)
        {
            _logger.LogError(ex, "Cosmos DB error for ID {Id}", id);
            throw;
        }
    }

    public async Task<(IEnumerable<Item>, string?)> GetItemsAsync(string? queryString, string? continuationToken, int pageSize = 10)
    {
        try
        {
            var query = _container.GetItemQueryIterator<Item>(
                new QueryDefinition(queryString),
                continuationToken,
                new QueryRequestOptions { MaxItemCount = pageSize });

            var response = await query.ReadNextAsync();
            return (response.Resource, response.ContinuationToken);
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
        {
            _logger.LogWarning(ex, "Rate limited. Retry after {RetryAfter}", ex.RetryAfter);
            throw;
        }
        catch (CosmosException ex)
        {
            _logger.LogError(ex, "Error during paged query");
            throw;
        }
    }

    public async Task AddItemAsync(Item newItem)
    {
        if (string.IsNullOrWhiteSpace(newItem.Id))
            newItem.Id = Guid.NewGuid().ToString();
        await _container.CreateItemAsync(newItem, new PartitionKey(newItem.Id));
    }

    public async Task UpdateItemAsync(string id, Item updatedItem)
    {
        updatedItem.Id = id;
        await _container.UpsertItemAsync(updatedItem, new PartitionKey(id));
    }

    public async Task DeleteItemAsync(string id)
    {
        await _container.DeleteItemAsync<Item>(id, new PartitionKey(id));
    }
}