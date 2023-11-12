using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;

public class TroopApi
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("level")]
    public int Level { get; set; }

    [JsonProperty("maxLevel")]
    public int MaxLevel { get; set; }

    [JsonProperty("village")]
    public string Village { get; set; }

    [JsonProperty("superTroopIsActive")]
    public bool? SuperTroopIsActivated { get; set; }
}