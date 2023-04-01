using UserInterestsAPIService.Models;
using Microsoft.Azure.Cosmos;

namespace UserInterestsAPIService.Services;

    public class IntrstsCosmosService : I_IntrstsCosmosService
    {

    private readonly Container _container;
    public IntrstsCosmosService(CosmosClient cosmosClient,
    string databaseName,
    string containerName)
    {
        _container = cosmosClient.GetContainer(databaseName, containerName);
    }

    public async Task<List<Intrsts>> Get(string sqlCosmosQuery)
    {
        var query = _container.GetItemQueryIterator<Intrsts>(new QueryDefinition(sqlCosmosQuery));

        List<Intrsts> result = new List<Intrsts>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            result.AddRange(response);
        }

        return result;
    }

    public async Task<Intrsts> AddAsync(Intrsts newIntrsts)
    {
        var item = await _container.CreateItemAsync<Intrsts>(newIntrsts, new PartitionKey(newIntrsts.interests));
        return item;
    }

    public async Task<Intrsts> Update(Intrsts IntrstsToUpdate)
    {
        var item = await _container.UpsertItemAsync<Intrsts>(IntrstsToUpdate, new PartitionKey(IntrstsToUpdate.interests));
        return item;
    }

    public async Task Delete(string id, string interests)
    {
        await _container.DeleteItemAsync<Intrsts>(id, new PartitionKey(interests));
    }






}

