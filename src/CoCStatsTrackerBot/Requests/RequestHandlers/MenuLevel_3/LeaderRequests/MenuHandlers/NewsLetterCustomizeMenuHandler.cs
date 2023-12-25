using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class NewsLetterCustomizeMenuHandler : BaseRequestHandler
{
    public NewsLetterCustomizeMenuHandler()
    {
        Header = "Настройки рассылки";
        HandlerMenuLevel = MenuLevel.LeaderNewsLetterCustomize3;
    }
}