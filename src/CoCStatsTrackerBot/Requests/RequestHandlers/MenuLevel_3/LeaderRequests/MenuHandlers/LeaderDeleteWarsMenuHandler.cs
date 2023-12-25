using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteWarsMenuHandler : BaseRequestHandler
{
    public LeaderDeleteWarsMenuHandler()
    {
        Header = "Удаление войн";
        HandlerMenuLevel = MenuLevel.LeaderDeleteClanWarsMenu3;
    }
}