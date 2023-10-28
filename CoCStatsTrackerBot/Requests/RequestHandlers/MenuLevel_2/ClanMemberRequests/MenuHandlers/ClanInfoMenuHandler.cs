using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanInfoMenuHandler : BaseRequestHandler
{
    public ClanInfoMenuHandler()
    {
        Header = "Клан";
        HandlerMenuLevel = MenuLevels.PlayerInfo2;
    }
}
