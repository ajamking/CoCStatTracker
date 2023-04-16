using CoCApiDealer.RequestsSettings;
using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

public class CapitalRaidsRequest : BaseApiRequest
{
    public async Task<RaidsApi> CallApi(string clanTag, int limit = 0)
    {
        try
        {
            var requestType = AllowedRequests.CapitalRaids;

            var apiRequestResult = await new ApiRequestBuilder(_httpClient, clanTag, requestType, limit).CallApi();

            var capitalRaidsInfo = JsonConvert.DeserializeObject<RaidsApi>(apiRequestResult);

            if (capitalRaidsInfo == null)
            {
                throw new Exception("Nothing came from API");
            }
            else
            {
                return capitalRaidsInfo;
            }

        }
        catch (Exception ex)
        {

            throw new ApiErrorException(ex.Message);
        }
    }
}