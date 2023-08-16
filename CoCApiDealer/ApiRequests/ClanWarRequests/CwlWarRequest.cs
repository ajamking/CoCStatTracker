using CoCApiDealer.RequestsSettings;
using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

public class CwlWarRequest : BaseApiRequest
{
    public async Task<ClanWarApi> CallApi(string cwlWarTag)
    {
        try
        {
            var requestType = AllowedRequests.CurrentCwlIndividualWar;

            var apiRequestResult = await new ApiRequestBuilder(_httpClient, cwlWarTag, requestType).CallApi();

            var currentCwlWar = JsonConvert.DeserializeObject<ClanWarApi>(apiRequestResult);

            if (currentCwlWar == null)
            {
                throw new Exception("Nothing came from API");
            }
            else
            {
                return currentCwlWar;
            }

        }
        catch (Exception ex)
        {
            throw new ApiErrorException(ex);
        }
    }
}
