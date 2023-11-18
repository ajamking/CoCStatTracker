using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

// Применения пока нет. Вся информация об игроках получается через PlayerRequest
public class ClanMembersRequest : BaseApiRequest
{
    public static async Task<ClanMembersApi> CallApi(string clanTag)
    {
        try
        {
            var requestType = AllowedRequests.ClanMembers;

            var apiRequestResult = await new ApiRequestBuilder(HttpClient, clanTag, requestType).CallApi();

            ApiInMaintenanceException.ThrowByPredicate(() => apiRequestResult.Contains("inMaintenance"), "Api is at maintenance, cant get searching info.");

            var clanMembers = JsonConvert.DeserializeObject<ClanMembersApi>(apiRequestResult);

            ApiNullOrEmtyResponseException.ThrowByPredicate(() => clanMembers == null || clanMembers.Members.Length == 0,
                    "ClanMembersRequest is failed, Nothing came from API");

            return clanMembers;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}