using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

public class ClanInfoRequest : BaseApiRequest
{
    public async Task<ClanApi> CallApi(string clanTag)
    {
        try
        {
            var requestType = AllowedRequests.Clan;

            var apiRequestResult = await new ApiRequestBuilder(_httpClient, clanTag, requestType).CallApi();

            var clanInfo = JsonConvert.DeserializeObject<ClanApi>(apiRequestResult);

            ApiNullOrEmtyResponseException.ThrowByPredicate(() => clanInfo == null || clanInfo.Tag == null,
                    "ClanInfoRequest is failed, Nothing came from API");

            return clanInfo;
        }
        catch (ApiNullOrEmtyResponseException ex)
        {
            return null;
        }
    }
}