using CoCApiDealer.RequestsSettings;
using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

public class WarLogRequest : BaseApiRequest
{
    public async Task<WarLogApi> CallApi(string clanTag, int limit = 0)
    {
        try
        {
            var requestType = AllowedRequests.WarLog;

            var apiRequestResult = await new ApiRequestBuilder(_httpClient, clanTag, requestType, limit).CallApi();

            var warLog = JsonConvert.DeserializeObject<WarLogApi>(apiRequestResult);

            if (warLog == null)
            {
                throw new Exception("Nothing came from API");
            }
            else
            {
                return warLog;
            }

        }
        catch (Exception ex)
        {

            throw new ApiErrorException(ex.Message);
        }
    }
}
