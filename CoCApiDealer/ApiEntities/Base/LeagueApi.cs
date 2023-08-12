using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;

public class LeagueApi
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}
