using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanWarHistoryMenuHandler : BaseRequestHandler
{
    public ClanWarHistoryMenuHandler()
    {
        Header = "История войн";
        HandlerMenuLevel = MenuLevel.ClanWarsHistory3;
    }
}
