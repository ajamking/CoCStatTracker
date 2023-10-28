using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanMemberMenuHandler : BaseRequestHandler
{
    public ClanMemberMenuHandler()
    {
        Header = "Член клана";
        HandlerMenuLevel = MenuLevels.Member1;
    }
}
