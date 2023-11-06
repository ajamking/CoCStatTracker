using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

// Применения пока нет. Вся информация об игроках получается через PlayerRequest
public class ClanMembersRequest : BaseApiRequest
{
    public async Task<ClanMembersApi> CallApi(string clanTag)
    {
        try
        {
            var requestType = AllowedRequests.ClanMembers;

            var apiRequestResult = await new ApiRequestBuilder(_httpClient, clanTag, requestType).CallApi();

            var clanMembers = JsonConvert.DeserializeObject<ClanMembersApi>(apiRequestResult);

            ApiNullOrEmtyResponseException.ThrowByPredicate(() => clanMembers == null || clanMembers.Members.Count() == 0,
                    "ClanMembersRequest is failed, Nothing came from API");

            return clanMembers;
        }
        catch (ApiNullOrEmtyResponseException ex)
        {
            return null;
        }
    }
}