using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class DeveloperMenuHandler : BaseRequestHandler
{
    public DeveloperMenuHandler()
    {
        Header = "Меню создателя";
        HandlerMenuLevel = MenuLevel.DeveloperMenu2;
    }
}
