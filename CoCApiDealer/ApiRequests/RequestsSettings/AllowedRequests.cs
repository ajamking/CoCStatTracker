namespace CoCApiDealer.ApiRequests;

public static class AllowedRequests
{
    public static AllowedRequest CurrentCwl { get; } = new("clans", "currentwar/leaguegroup");
    public static AllowedRequest CurrentCwlIndividualWar { get; } = new("clanwarleagues/wars", "");
    public static AllowedRequest WarLog { get; } = new("clans", "warlog");
    public static AllowedRequest CurrentWar { get; } = new("clans", "currentwar");
    public static AllowedRequest Clan { get; } = new("clans", "");
    public static AllowedRequest ClanMembers { get; } = new("clans", "members"); // Применения пока не вижу
    public static AllowedRequest CapitalRaids { get; } = new("clans", "capitalraidseasons");
    public static AllowedRequest Player { get; } = new("players", "");
}

public class AllowedRequest
{
    public string SearchBy { get; set; }

    public string LastEndPointWord { get; set; }

    public AllowedRequest(string searchBy, string lastEndpointWord)
    {
        SearchBy = searchBy;
        LastEndPointWord = lastEndpointWord;
    }
}