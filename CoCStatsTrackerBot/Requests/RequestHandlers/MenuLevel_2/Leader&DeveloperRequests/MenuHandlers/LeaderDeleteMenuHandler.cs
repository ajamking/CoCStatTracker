using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteMenuHandler : BaseRequestHandler
{
    public LeaderDeleteMenuHandler()
    {
        Header = "Удаление";
        HandlerMenuLevel = MenuLevel.LeaderDeleteMenu2;
    }
}