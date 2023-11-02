using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class D1CapitalPeakRH : BaseRequestHandler
{
    public D1CapitalPeakRH()
    {
        Header = "Столичный пик";
        HandlerMenuLevel = MenuLevel.CurrentDistrictStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.DistrictType = DistrictType.Capital_Peak;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}
