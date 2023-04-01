using UserInterestsAPIService.Models;

namespace UserInterestsAPIService.Services;

    public interface I_IntrstsCosmosService
    {
        Task<List<Intrsts>> Get(string sqlCosmosQuery);
        Task<Intrsts> AddAsync(Intrsts newIntrsts);
        Task<Intrsts> Update(Intrsts IntrstsToUpdate);

        Task Delete(string id, string make);
    }

