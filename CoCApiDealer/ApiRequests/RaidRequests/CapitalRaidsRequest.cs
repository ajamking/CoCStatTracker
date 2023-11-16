using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

public class CapitalRaidsRequest : BaseApiRequest
{
    public static async Task<RaidsApi> CallApi(string clanTag, int limit = 0)
    {
        try
        {
            var requestType = AllowedRequests.CapitalRaids;

            var apiRequestResult = await new ApiRequestBuilder(HttpClient, clanTag, requestType, limit).CallApi();

            var capitalRaidsInfo = JsonConvert.DeserializeObject<RaidsApi>(apiRequestResult);

            ApiNullOrEmtyResponseException.ThrowByPredicate(() => capitalRaidsInfo == null || capitalRaidsInfo.RaidsInfo == null,
              "CapitalRaidsRequest is failed, Nothing came from API");

            return capitalRaidsInfo;
        }
        catch (ApiNullOrEmtyResponseException)
        {
            return null;
        }
    }
}