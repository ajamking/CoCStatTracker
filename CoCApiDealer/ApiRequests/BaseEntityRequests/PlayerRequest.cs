using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

public class PlayerRequest : BaseApiRequest
{
    public static async Task<PlayerApi> CallApi(string clanTag)
    {
        try
        {
            var requestType = AllowedRequests.Player;

            var apiRequestResult = await new ApiRequestBuilder(HttpClient, clanTag, requestType).CallApi();

            var playerInfo = JsonConvert.DeserializeObject<PlayerApi>(apiRequestResult);

            ApiNullOrEmtyResponseException.ThrowByPredicate(() => playerInfo == null || playerInfo.Tag == null,
                    "PlayerRequest is failed, Nothing came from API");

            return playerInfo;

        }
        catch (ApiNullOrEmtyResponseException)
        {
            return null;
        }
    }
}