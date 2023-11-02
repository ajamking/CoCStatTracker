using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class D2BarbariansRH : BaseRequestHandler
{
    public D2BarbariansRH()
    {
        Header = "Лагерь варваров";
        HandlerMenuLevel = MenuLevel.CurrentDistrictStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.DistrictType = DistrictType.Barbarian_Camp;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}
