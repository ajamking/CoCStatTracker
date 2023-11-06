using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

public class CurrentWarRequest : BaseApiRequest
{
    public async Task<ClanWarApi> CallApi(string clanTag)
    {
        try
        {
            var requestType = AllowedRequests.CurrentWar;

            var apiRequestResult = await new ApiRequestBuilder(_httpClient, clanTag, requestType).CallApi();

            var currentWar = JsonConvert.DeserializeObject<ClanWarApi>(apiRequestResult);

            ApiNullOrEmtyResponseException.ThrowByPredicate(() => currentWar == null || currentWar.StartTime == null,
                    "CurrentWarRequest is failed, Nothing came from API");

            return currentWar;

        }
        catch (ApiNullOrEmtyResponseException ex)
        {
            return null;
        }
    }
}