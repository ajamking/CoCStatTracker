﻿using CoCStatsTracker.ApiEntities;
using Newtonsoft.Json;

namespace CoCApiDealer.ApiRequests;

public class CurrentWarRequest : BaseApiRequest
{
    public static async Task<ClanWarApi> CallApi(string clanTag)
    {
        try
        {
            var requestType = AllowedRequests.CurrentWar;

            var apiRequestResult = await new ApiRequestBuilder(HttpClient, clanTag, requestType).CallApi();

            ApiInMaintenanceException.ThrowByPredicate(() => apiRequestResult.Contains("inMaintenance"), "Api is at maintenance, cant get searching info.");

            var currentWar = JsonConvert.DeserializeObject<ClanWarApi>(apiRequestResult);

            ApiNullOrEmtyResponseException.ThrowByPredicate(() => currentWar == null || currentWar.StartTime == null,
                    "CurrentWarRequest is failed, Nothing came from API");

            return currentWar;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}