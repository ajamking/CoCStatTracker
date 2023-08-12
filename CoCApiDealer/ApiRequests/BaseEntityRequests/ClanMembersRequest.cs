using CoCApiDealer.RequestsSettings;
using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

// Применения пока нет.
public class ClanMembersRequest : BaseApiRequest
{
    public async Task<ClanMembersApi> CallApi(string clanTag)
    {
        try
        {
            var requestType = AllowedRequests.ClanMembers;

            var apiRequestResult = await new ApiRequestBuilder(_httpClient, clanTag, requestType).CallApi();

            var clanMember = JsonConvert.DeserializeObject<ClanMembersApi>(apiRequestResult);

            if (clanMember == null)
            {
                throw new Exception("Nothing came from API");
            }
            else
            {
                return clanMember;
            }

        }
        catch (Exception ex)
        {

            throw new ApiErrorException(ex.Message);
        }
    }
}