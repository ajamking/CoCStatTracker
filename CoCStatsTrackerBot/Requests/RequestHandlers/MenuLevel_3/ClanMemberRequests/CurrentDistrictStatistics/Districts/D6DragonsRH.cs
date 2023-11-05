using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class D6DragonsRH : BaseRequestHandler
{
    public D6DragonsRH()
    {
        Header = "Драконьи утесы";
        HandlerMenuLevel = MenuLevel.CurrentDistrictStatistics3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.DistrictType = ADistrictType.Dragon_Cliffs;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}
