using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class D8SkeletonsRH : BaseRequestHandler
{
    public D8SkeletonsRH()
    {
        Header = "Парк скелетов";
        HandlerMenuLevel = MenuLevels.CurrentDistrictStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.DistrictType = DistrictType.Skeleton_Park;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}
