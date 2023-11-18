using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class D8SkeletonsRH : BaseRequestHandler
{
    public D8SkeletonsRH()
    {
        Header = "Парк скелетов";
        HandlerMenuLevel = MenuLevel.CurrentDistrictStatistics3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.DistrictType = DistrictType.Skeleton_Park;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}