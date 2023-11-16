using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

//Пока не используется. Для получения полной информации о войнах, нужно брать именно последнюю, варлог бесполезен.
public class WarLogRequest : BaseApiRequest
{
    public static async Task<WarLogApi> CallApi(string clanTag, int limit = 1)
    {
        try
        {
            var requestType = AllowedRequests.WarLog;

            var apiRequestResult = await new ApiRequestBuilder(HttpClient, clanTag, requestType, limit).CallApi();

            var warLog = JsonConvert.DeserializeObject<WarLogApi>(apiRequestResult);

            ApiNullOrEmtyResponseException.ThrowByPredicate(() => warLog == null,
               "WarLogRequest is failed, Nothing came from API");

            return warLog;
        }
        catch (ApiNullOrEmtyResponseException)
        {
            return null;
        }
    }
}