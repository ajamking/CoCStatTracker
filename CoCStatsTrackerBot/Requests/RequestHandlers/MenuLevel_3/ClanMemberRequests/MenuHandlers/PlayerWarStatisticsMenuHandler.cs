using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class PlayerWarStatisticsMenuHandler : BaseRequestHandler
{
    public PlayerWarStatisticsMenuHandler()
    {
        Header = "Показатели войн";
        HandlerMenuLevel = MenuLevels.PlayerWarStatistics3;
    }
}
