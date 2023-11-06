namespace CoCApiDealer.ApiRequests;

public static class AllowedRequests
{
    public static AllowedRequest CurrentCwl = new AllowedRequest("clans", "currentwar/leaguegroup");
    public static AllowedRequest CurrentCwlIndividualWar = new AllowedRequest("clanwarleagues/wars", "");

    public static AllowedRequest WarLog = new AllowedRequest("clans", "warlog");
    public static AllowedRequest CurrentWar = new AllowedRequest("clans", "currentwar");

    public static AllowedRequest Clan = new AllowedRequest("clans", "");
    public static AllowedRequest ClanMembers = new AllowedRequest("clans", "members"); // Применения пока не вижу

    public static AllowedRequest CapitalRaids = new AllowedRequest("clans", "capitalraidseasons");

    public static AllowedRequest Player = new AllowedRequest("players", "");
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
