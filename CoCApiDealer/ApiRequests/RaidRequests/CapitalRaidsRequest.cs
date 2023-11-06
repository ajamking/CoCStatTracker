using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

public class CapitalRaidsRequest : BaseApiRequest
{
    public async Task<RaidsApi> CallApi(string clanTag, int limit = 0)
    {
        try
        {
            var requestType = AllowedRequests.CapitalRaids;

            var apiRequestResult = await new ApiRequestBuilder(_httpClient, clanTag, requestType, limit).CallApi();

            var capitalRaidsInfo = JsonConvert.DeserializeObject<RaidsApi>(apiRequestResult);

            ApiNullOrEmtyResponseException.ThrowByPredicate(() => capitalRaidsInfo == null || capitalRaidsInfo.RaidsInfo == null,
              "CapitalRaidsRequest is failed, Nothing came from API");

            return capitalRaidsInfo;
        }
        catch (ApiNullOrEmtyResponseException ex)
        {
            return null;
        }
    }
}