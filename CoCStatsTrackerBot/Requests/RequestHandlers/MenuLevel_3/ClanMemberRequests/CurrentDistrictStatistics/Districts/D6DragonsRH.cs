using CoCStatsTrackerBot.BotMenues;

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
        parameters.DistrictType = DistrictType.Dragon_Cliffs;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}