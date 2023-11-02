using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class PlayerRaidStatisticsMenuHandler : BaseRequestHandler
{
    public PlayerRaidStatisticsMenuHandler()
    {
        Header = "Показатели рейдов";
        HandlerMenuLevel = MenuLevel.PlayerRaidStatistics3;
    }
}
