using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;

public class AttackedClanInfoApi
{
    [JsonProperty("tag")]
    public string Tag { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("level")]
    public int Level { get; set; }
}
