using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;

public class DistrictApi
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("districtHallLevel")]
    public int DistrictLevel { get; set; }

    [JsonProperty("destructionPercent")]
    public int DestructionPercent { get; set; } 

    [JsonProperty("stars")]
    public int StarsCount { get; set; }  //Не используем

    [JsonProperty("attackCount")]
    public int AttackCount { get; set; } 

    [JsonProperty("totalLooted")]
    public int TotalLooted { get; set; }

    [JsonProperty("attacks")]
    public AttackOnDistrictApi[] MemberAttacks { get; set; }
}