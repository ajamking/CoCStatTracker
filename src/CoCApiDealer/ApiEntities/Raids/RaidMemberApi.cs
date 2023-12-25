using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;

public class RaidMemberApi
{
    [JsonProperty("tag")]
    public string Tag { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("attacks")]
    public int AttacksCount { get; set; }

    [JsonProperty("capitalResourcesLooted")]
    public int CapitalResourcesLooted { get; set; }
}