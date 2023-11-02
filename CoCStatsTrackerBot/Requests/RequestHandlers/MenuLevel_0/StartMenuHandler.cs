using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class StartMenuHandler : BaseRequestHandler
{
    public StartMenuHandler()
    {
        Header = "/start";
        HandlerMenuLevel = MenuLevel.Main0;
    }
}
