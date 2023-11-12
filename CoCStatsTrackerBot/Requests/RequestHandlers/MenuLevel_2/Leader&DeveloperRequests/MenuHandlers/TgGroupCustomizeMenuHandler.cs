using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class TgGroupCustomizeMenuHandler : BaseRequestHandler
{
    public TgGroupCustomizeMenuHandler()
    {
        Header = "Настройки ТГ группы";
        HandlerMenuLevel = MenuLevel.LeaderTgGroupCustomize2;
    }
}