using CoCApiDealer.RequestsSettings;
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

            if (playerInfo == null)
            {
                throw new Exception("Nothing came from API");
            }
            else
            {
                return playerInfo;
            }

        }
        catch (Exception ex)
        {

            throw new ApiErrorException(ex);
        }
    }
}
