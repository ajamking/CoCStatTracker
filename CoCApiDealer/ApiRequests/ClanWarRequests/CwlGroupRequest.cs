using CoCApiDealer.RequestsSettings;
using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

public class CwlGroupRequest : BaseApiRequest
{
    public async Task<CwlGroupApi> CallApi(string clanTag)
    {
        try
        {
            var requestType = AllowedRequests.CurrentCwl;

            var apiRequestResult = await new ApiRequestBuilder(_httpClient, clanTag, requestType).CallApi();

            var cwlGroup = JsonConvert.DeserializeObject<CwlGroupApi>(apiRequestResult);

            if (cwlGroup.Season == null && 
                cwlGroup.ParticipantClans == null && 
                cwlGroup.Rounds == null && 
                cwlGroup.State == null)
            {
                throw new Exception("Nothing came from API");
            }
            else
            {
                return cwlGroup;
            }

        }
        catch (Exception ex)
        {
            throw new ApiErrorException(ex);
        }
    }
}
