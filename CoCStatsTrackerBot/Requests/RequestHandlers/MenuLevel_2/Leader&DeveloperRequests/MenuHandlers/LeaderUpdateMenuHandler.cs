using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class LeaderUpdateMenuHandler : BaseRequestHandler
{
    public LeaderUpdateMenuHandler()
    {
        Header = "Обновление";
        HandlerMenuLevel = MenuLevel.LeaderUpdateMenu2;
    }
}