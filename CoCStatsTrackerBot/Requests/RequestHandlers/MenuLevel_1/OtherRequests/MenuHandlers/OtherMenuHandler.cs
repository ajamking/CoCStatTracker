using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class OtherMenuHandler : BaseRequestHandler
{
    public OtherMenuHandler()
    {
        Header = "Прочее";
        HandlerMenuLevel = MenuLevel.Other1;
    }
}