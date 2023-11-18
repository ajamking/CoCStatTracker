using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class ClanCurrentRaidInfoMenuHandler : BaseRequestHandler
{
    public ClanCurrentRaidInfoMenuHandler()
    {
        Header = "Текущий рейд";
        HandlerMenuLevel = MenuLevel.CurrentRaidInfo2;
    }
}