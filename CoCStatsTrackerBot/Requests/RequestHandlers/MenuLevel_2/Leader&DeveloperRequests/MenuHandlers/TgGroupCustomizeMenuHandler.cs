using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class TgGroupCustomizeMenuHandler : BaseRequestHandler
{
    public TgGroupCustomizeMenuHandler()
    {
        Header = "Настройки ТГ группы";
        HandlerMenuLevel = MenuLevel.LeaderTgGroupCustomize2;
    }
}