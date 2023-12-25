using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class LeaderAddMenuHandler : BaseRequestHandler
{
    public LeaderAddMenuHandler()
    {
        Header = "Добавление";
        HandlerMenuLevel = MenuLevel.LeaderAddMenu2;
    }
}