using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class D3WizzardsRH : BaseRequestHandler
{
    public D3WizzardsRH()
    {
        Header = "Долина колдунов";
        HandlerMenuLevel = MenuLevels.CurrentDistrictStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.DistrictType = DistrictType.Wizard_Valley;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}
