using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanMemberMenuHandler : BaseRequestHandler
{
    public ClanMemberMenuHandler()
    {
        Header = "Основные функции";
        HandlerMenuLevel = MenuLevel.Member1;
    }
}
