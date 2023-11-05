using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class D7GolemsRH : BaseRequestHandler
{
    public D7GolemsRH()
    {
        Header = "Карьер големов";
        HandlerMenuLevel = MenuLevel.CurrentDistrictStatistics3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.DistrictType = ADistrictType.Golem_Quarry;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}
