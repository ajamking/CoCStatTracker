using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class ClanCurrentWarInfoMenuHandler : BaseRequestHandler
{
    public ClanCurrentWarInfoMenuHandler()
    {
        Header = "Текущая война";
        HandlerMenuLevel = MenuLevel.CurrentWarInfo2;
    }
}