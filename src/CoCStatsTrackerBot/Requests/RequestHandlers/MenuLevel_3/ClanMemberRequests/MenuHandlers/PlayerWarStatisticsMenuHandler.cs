using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class PlayerWarStatisticsMenuHandler : BaseRequestHandler
{
    public PlayerWarStatisticsMenuHandler()
    {
        Header = "Показатели войн";
        HandlerMenuLevel = MenuLevel.PlayerWarStatistics3;
    }
}