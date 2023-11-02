using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanCurrentRaidInfoMenuHandler : BaseRequestHandler
{
    public ClanCurrentRaidInfoMenuHandler()
    {
        Header = "Текущий рейд";
        HandlerMenuLevel = MenuLevel.CurrentRaidInfo2;
    }
}
