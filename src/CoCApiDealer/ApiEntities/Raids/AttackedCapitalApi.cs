using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;

public class AttackedCapitalApi
{
    [JsonProperty("defender")]
    public AttackedClanInfoApi DefenderClan { get; set; }

    [JsonProperty("attackCount")]
    public int AttackCount { get; set; }

    [JsonProperty("districtCount")]
    public int DistrictCount { get; set; }

    [JsonProperty("districtsDestroyed")]
    public int DistrictsDestroyedCount { get; set; }

    [JsonProperty("districts")]
    public DistrictApi[] DestroyedDistricts { get; set; }
}