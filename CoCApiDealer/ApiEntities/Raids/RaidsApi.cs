using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;

public class RaidsApi
{
    [JsonProperty("items")]
    public RaidApi[] RaidsInfo { get; set; }
}

public class RaidApi
{
    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("startTime")]
    public string StartTime { get; set; }

    [JsonProperty("endTime")]
    public string EndTime { get; set; }

    [JsonProperty("capitalTotalLoot")]
    public int CapitalTotalLoot { get; set; }

    [JsonProperty("raidsCompleted")]
    public int RaidsCompleted { get; set; }

    [JsonProperty("totalAttacks")]
    public int TotalAttacks { get; set; }

    [JsonProperty("enemyDistrictsDestroyed")]
    public int EnemyDistrictsDestroyed { get; set; }

    [JsonProperty("offensiveReward")]
    public int OffensiveRewardBase { get; set; }

    [JsonProperty("defensiveReward")]
    public int DefensiveReward { get; set; }

    [JsonProperty("members")]
    public RaidMemberApi[] RaidMembers { get; set; }

    [JsonProperty("attackLog")]
    public AttackedCapitalApi[] AttackedCapitals { get; set; }

    [JsonProperty("defenseLog")]
    public DefenseApi[] RaidDefenses { get; set; }
}