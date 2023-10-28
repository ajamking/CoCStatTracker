using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class D5BuildersRH : BaseRequestHandler
{
    public D5BuildersRH()
    {
        Header = "Мастерская строителя";
        HandlerMenuLevel = MenuLevels.CurrentDistrictStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.DistrictType = DistrictType.Builders_Workshop;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}
