﻿using CoCStatsTracker.ApiEntities;
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

            ApiNullOrEmtyResponseException.ThrowByPredicate(() => currentCwlWar == null || currentCwlWar.StartTime == null,
                   "CwlWarRequest is failed, Nothing came from API");

            return currentCwlWar;
        }
        catch (ApiNullOrEmtyResponseException ex)
        {
            return null;
        }
    }
}
