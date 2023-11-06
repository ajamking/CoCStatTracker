using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;

public class ClanApi
{
    [JsonProperty("tag")]
    public string Tag { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("clanLevel")]
    public int ClanLevel { get; set; }

    [JsonProperty("clanPoints")]
    public int ClanPoints { get; set; }

    [JsonProperty("clanVersusPoints")]
    public int ClanVersusPoints { get; set; }

    [JsonProperty("clanCapitalPoints")]
    public int ClanCapitalPoints { get; set; }

    [JsonProperty("capitalLeague")]
    public LeagueApi? CapitalLeague { get; set; }

    [JsonProperty("requiredTrophies")]
    public int RequiredTrophies { get; set; }

    [JsonProperty("warWinStreak")]
    public int WarWinStreak { get; set; }

    [JsonProperty("warWins")]
    public int WarWins { get; set; }

    [JsonProperty("warTies")]
    public int WarTIes { get; set; }

    [JsonProperty("warLosses")]
    public int WarLoses { get; set; }

    [JsonProperty("isWarLogPublic")]
    public bool IsWarLogPublic { get; set; }

    [JsonProperty("warLeague")]
    public LeagueApi WarLeague { get; set; }

    [JsonProperty("clanCapital")]
    public ClanCapitalApi? ClanCapital { get; set; }

    [JsonProperty("members")]
    public int MembersCount { get; set; }

    [JsonProperty("memberList")]
    public ClanMemberApi[]? Members { get; set; }
}

public class ClanCapitalApi
{
    [JsonProperty("capitalHallLevel")]
    public int CapitalHallLevel { get; set; }


}


