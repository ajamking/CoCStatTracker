using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanWarHistoryMenuHandler : BaseRequestHandler
{
    public ClanWarHistoryMenuHandler()
    {
        Header = "История войн";
        HandlerMenuLevel = MenuLevels.ClanWarsHistory3;
    }
}
