using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class D2BarbariansRH : BaseRequestHandler
{
    public D2BarbariansRH()
    {
        Header = "Лагерь варваров";
        HandlerMenuLevel = MenuLevel.CurrentDistrictStatistics3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.DistrictType = ADistrictType.Barbarian_Camp;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}
