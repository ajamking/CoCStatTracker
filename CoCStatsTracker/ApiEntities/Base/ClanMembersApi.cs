using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;

//Весь класс пока повис, для заоплнения БД используются поочередные запросы по тегу каждого игрока.
public class ClanMembersApi
{
    [JsonProperty("items")]
    public ClanMemberApi[] Members { get; set; }

}

public class ClanMemberApi
{
    [JsonProperty("tag")]
    public string Tag { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("expLevel")]
    public int ExpLevel { get; set; }

    [JsonProperty("league")]
    public LeagueApi League { get; set; }

    [JsonProperty("trophies")]
    public int Trophies { get; set; }

    [JsonProperty("versusTrophies")]
    public int VersusTrophies { get; set; }

    /*Уникальное поле, которого нет в запросе по тегу игрока, 
     * в домейне также отсутствует. Возможно понадобится позже*/
    [JsonProperty("clanRank")]
    public int RankInClan { get; set; }

    [JsonProperty("donations")]
    public int DonationsSent { get; set; }

    [JsonProperty("donationsReceived")]
    public int DonationsReceived { get; set; }
}
