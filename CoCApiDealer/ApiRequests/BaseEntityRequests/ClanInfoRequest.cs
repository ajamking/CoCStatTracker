using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

public class ClanInfoRequest : BaseApiRequest
{
    public static async Task<ClanApi> CallApi(string clanTag)
    {
        try
        {
            var requestType = AllowedRequests.Clan;

            var apiRequestResult = await new ApiRequestBuilder(HttpClient, clanTag, requestType).CallApi();

            ApiInMaintenanceException.ThrowByPredicate(() => apiRequestResult.Contains("inMaintenance"), "Api is at maintenance, cant get searching info.");

            var clanInfo = JsonConvert.DeserializeObject<ClanApi>(apiRequestResult);

            ApiNullOrEmtyResponseException.ThrowByPredicate(() => clanInfo == null || clanInfo.Tag == null,
                    "ClanInfoRequest is failed, Nothing came from API");

            return clanInfo;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}