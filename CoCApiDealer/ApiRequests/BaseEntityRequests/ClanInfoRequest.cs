using CoCApiDealer.RequestsSettings;
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

            if (clanInfo == null)
            {
                throw new Exception("Nothing came from API");
            }
            else
            {
                return clanInfo;
            }

        }
        catch (Exception ex)
        {

            throw new ApiErrorException(ex);
        }

    }
}