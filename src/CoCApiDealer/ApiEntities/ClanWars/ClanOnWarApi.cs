using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;

public class ClanOnWarApi
{
    [JsonProperty("tag")]
    public string Tag { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("clanLevel")]
    public int ClanLevel { get; set; }

    [JsonProperty("attacks")]
    public int AttacksCount { get; set; }

    [JsonProperty("stars")]
    public int StarsCount { get; set; }

    [JsonProperty("destructionPercentage")]
    public double DestructionPercentage { get; set; }

    [JsonProperty("members")]
    public WarMemberApi[] WarMembers { get; set; }
}