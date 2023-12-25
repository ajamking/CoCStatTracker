using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;

public class PlayerApi
{
    [JsonProperty("tag")]
    public string Tag { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("townHallLevel")]
    public int TownHallLevel { get; set; }

    [JsonProperty("townHallWeaponLevel")]
    public int TownHallWeaponLevel { get; set; }

    [JsonProperty("expLevel")]
    public int ExpLevel { get; set; }

    [JsonProperty("trophies")]
    public int Trophies { get; set; }

    [JsonProperty("bestTrophies")]
    public int BestTrophies { get; set; }

    [JsonProperty("warStars")]
    public int WarStars { get; set; }

    [JsonProperty("attackWins")]
    public int AttackWins { get; set; }

    [JsonProperty("defenseWins")]
    public int DefenseWins { get; set; }

    [JsonProperty("builderHallLevel")]
    public int BuilderHallLevel { get; set; }

    [JsonProperty("versusTrophies")]
    public int VersusTrophies { get; set; }

    [JsonProperty("bestVersusTrophies")]
    public int BestVersusTrophies { get; set; }

    [JsonProperty("versusBattleWins")]
    public int VersusBattleWins { get; set; }

    [JsonProperty("role")]
    public string RoleInClan { get; set; }

    [JsonProperty("warPreference")]
    public string WarPreference { get; set; } // can be in or out

    [JsonProperty("donations")]
    public int DonationsSent { get; set; }

    [JsonProperty("donationsReceived")]
    public int DonationsReceived { get; set; }

    [JsonProperty("clanCapitalContributions")]
    public int ClanCapitalContributions { get; set; }

    [JsonProperty("clan")]
    public ClanShortInfoApi Clan { get; set; }

    [JsonProperty("league")]
    public LeagueApi League { get; set; }

    [JsonProperty("troops")]
    public TroopApi[] Troops { get; set; }

    [JsonProperty("heroes")]
    public TroopApi[] Heroes { get; set; }

    [JsonProperty("achievements")]
    public Achievement[] Achievements { get; set; }

}

public class Achievement
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("stars")]
    public int Stars { get; set; }

    [JsonProperty("value")]
    public int Value { get; set; }

    [JsonProperty("target")]
    public int Target { get; set; }

    [JsonProperty("info")]
    public string Info { get; set; }

    [JsonProperty("completionInfo")]
    public string CompletedInfo { get; set; }

    [JsonProperty("village")]
    public string Village { get; set; }
}