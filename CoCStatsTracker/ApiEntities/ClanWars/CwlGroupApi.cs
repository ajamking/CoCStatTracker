using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;
public class CwlGroupApi
{
    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("season")]
    public string Season { get; set; }

    [JsonProperty("clans")]
    public ClanShortInfoApi[] ParticipantClans { get; set; }

    [JsonProperty("rounds")]
    public CWLRoundApi[] Rounds { get; set; }
}

public class CWLRoundApi
{
    [JsonProperty("warTags")]
    public string[] WarTags { get; set; }
}
