using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class D4BaloonsRH : BaseRequestHandler
{
    public D4BaloonsRH()
    {
        Header = "Лагуна шаров";
        HandlerMenuLevel = MenuLevel.CurrentDistrictStatistics3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.DistrictType = ADistrictType.Balloon_Lagoon;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}
