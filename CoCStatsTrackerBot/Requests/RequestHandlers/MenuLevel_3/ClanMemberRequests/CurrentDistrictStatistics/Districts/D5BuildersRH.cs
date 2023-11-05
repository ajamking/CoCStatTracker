using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class D5BuildersRH : BaseRequestHandler
{
    public D5BuildersRH()
    {
        Header = "Мастерская строителя";
        HandlerMenuLevel = MenuLevel.CurrentDistrictStatistics3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.DistrictType = ADistrictType.Builders_Workshop;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}
