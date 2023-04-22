using CoCApiDealer.RequestsSettings;
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

            if (currentWar == null)
            {
                throw new Exception("Nothing came from API");
            }
            else
            {
                return currentWar;
            }

        }
        catch (Exception ex)
        {
            throw new ApiErrorException(ex.Message);
        }
    }
}