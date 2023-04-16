using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;
public class ClanWarApi
{
    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("teamSize")]
    public int TeamSize { get; set; }

    [JsonProperty("attacksPerMember")]
    public int AttacksPerMember { get; set; } // Only for non-cwl wars

    [JsonProperty("preparationStartTime")]
    public string PreparationStartTime { get; set; }

    [JsonProperty("startTime")]
    public string StartTime { get; set; }

    [JsonProperty("endTime")]
    public string EndTime { get; set; }

    [JsonProperty("clan")]
    public ClanOnWarApi ClanResults { get; set; }

    [JsonProperty("opponent")]
    public ClanOnWarApi OpponentResults { get; set; }
}




