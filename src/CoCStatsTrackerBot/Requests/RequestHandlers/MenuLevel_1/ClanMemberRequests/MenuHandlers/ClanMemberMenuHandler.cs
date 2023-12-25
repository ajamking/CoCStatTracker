using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class ClanMemberMenuHandler : BaseRequestHandler
{
    public ClanMemberMenuHandler()
    {
        Header = "Основные функции";
        HandlerMenuLevel = MenuLevel.Member1;
    }
}