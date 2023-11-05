using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteRaidsMenuHandler : BaseRequestHandler
{
    public LeaderDeleteRaidsMenuHandler()
    {
        Header = "Удаление рейдов";
        HandlerMenuLevel = MenuLevel.LeaderDeleteRaidsMenu3;
    }
}