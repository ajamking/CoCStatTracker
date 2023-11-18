using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class LeaderMenuHandler : BaseRequestHandler
{
    public LeaderMenuHandler()
    {
        Header = "Интерфейс главы клана";
        HandlerMenuLevel = MenuLevel.Leader1;
    }
}