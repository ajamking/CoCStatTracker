using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

public class CwlGroupRequest : BaseApiRequest
{
    public static async Task<CwlGroupApi> CallApi(string clanTag)
    {
        try
        {
            var requestType = AllowedRequests.CurrentCwl;

            var apiRequestResult = await new ApiRequestBuilder(HttpClient, clanTag, requestType).CallApi();

            ApiInMaintenanceException.ThrowByPredicate(() => apiRequestResult.Contains("inMaintenance"), "Api is at maintenance, cant get searching info.");

            var cwlGroup = JsonConvert.DeserializeObject<CwlGroupApi>(apiRequestResult);

            ApiNullOrEmtyResponseException.ThrowByPredicate(() => (cwlGroup.Season == null ||
                cwlGroup.State == null ||
                cwlGroup.Rounds == null ||
                cwlGroup.ParticipantClans == null),
                "CwlGroupRequest is failed, Nothing came from API");

            return cwlGroup;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}