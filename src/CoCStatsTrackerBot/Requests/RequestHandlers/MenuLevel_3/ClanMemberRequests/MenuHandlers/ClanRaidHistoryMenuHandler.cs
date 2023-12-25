using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class ClanRaidHistoryMenuHandler : BaseRequestHandler
{
    public ClanRaidHistoryMenuHandler()
    {
        Header = "История рейдов";
        HandlerMenuLevel = MenuLevel.ClanRaidsHistory3;
    }
}