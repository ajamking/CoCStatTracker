using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;
public class WarLogApi
{
    [JsonProperty("items")]
    public WarLogWarInfoApi[] PreviousWars { get; set; }
}

public class WarLogWarInfoApi
{
    [JsonProperty("result")]
    public string WarResult { get; set; }

    [JsonProperty("endTime")]
    public string EndTime { get; set; }

    [JsonProperty("teamSize")]
    public int TeamSize { get; set; }

    [JsonProperty("attacksPerMember")]
    public int AttacksPerMember { get; set; }

    [JsonProperty("clan")]
    public WarLogClanApi ClanResults { get; set; }

    [JsonProperty("opponent")]
    public WarLogClanApi OpponentResults { get; set; }
}

public class WarLogClanApi
{
    [JsonProperty("tag")]
    public string ClanTag { get; set; }

    [JsonProperty("name")]
    public string ClanName { get; set; }

    [JsonProperty("clanLevel")]
    public int ClanLevel { get; set; }

    [JsonProperty("attacks")]
    public int AttacksCount { get; set; }

    [JsonProperty("stars")]
    public int StarsCount { get; set; }

    [JsonProperty("destructionPercentage")]
    public double AverageDestructionPercent { get; set; }

    [JsonProperty("expEarned")]
    public int ExpEarned { get; set; } = 0;
}