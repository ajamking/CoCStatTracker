using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class PlayerRaidStatisticsMenuHandler : BaseRequestHandler
{
    public PlayerRaidStatisticsMenuHandler()
    {
        Header = "Показатели рейдов";
        HandlerMenuLevel = MenuLevels.PlayerRaidStatistics3;
    }
}
