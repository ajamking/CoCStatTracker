using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanCurrentRaidInfoMenuHandler : BaseRequestHandler
{
    public ClanCurrentRaidInfoMenuHandler()
    {
        Header = "Текущий рейд";
        HandlerMenuLevel = MenuLevels.CurrentRaidInfo2;
    }
}
