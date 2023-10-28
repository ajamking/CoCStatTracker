using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanRaidHistoryMenuHandler : BaseRequestHandler
{
    public ClanRaidHistoryMenuHandler()
    {
        Header = "История рейдов";
        HandlerMenuLevel = MenuLevels.ClanRaidsHistory3;
    }
}
