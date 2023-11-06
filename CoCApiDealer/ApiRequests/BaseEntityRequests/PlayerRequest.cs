using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

public class PlayerRequest : BaseApiRequest
{
    public async Task<PlayerApi> CallApi(string clanTag)
    {
        try
        {
            var requestType = AllowedRequests.Player;

            var apiRequestResult = await new ApiRequestBuilder(_httpClient, clanTag, requestType).CallApi();

            var playerInfo = JsonConvert.DeserializeObject<PlayerApi>(apiRequestResult);

            ApiNullOrEmtyResponseException.ThrowByPredicate(() => playerInfo == null || playerInfo.Tag == null,
                    "PlayerRequest is failed, Nothing came from API");

            return playerInfo;

        }
        catch (ApiNullOrEmtyResponseException ex)
        {
            return null;
        }
    }
}