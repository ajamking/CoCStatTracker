using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class D6DragonsRH : BaseRequestHandler
{
    public D6DragonsRH()
    {
        Header = "Драконьи утесы";
        HandlerMenuLevel = MenuLevels.CurrentDistrictStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.DistrictType = DistrictType.Dragon_Cliffs;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}
