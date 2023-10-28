using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class StartMenuHandler : BaseRequestHandler
{
    public StartMenuHandler()
    {
        Header = "/start";
        HandlerMenuLevel = MenuLevels.Main0;
    }
}
