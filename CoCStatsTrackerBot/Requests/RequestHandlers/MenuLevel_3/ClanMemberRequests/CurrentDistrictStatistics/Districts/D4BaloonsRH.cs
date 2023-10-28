using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class D4BaloonsRH : BaseRequestHandler
{
    public D4BaloonsRH()
    {
        Header = "Лагуна шаров";
        HandlerMenuLevel = MenuLevels.CurrentDistrictStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.DistrictType = DistrictType.Balloon_Lagoon;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}
