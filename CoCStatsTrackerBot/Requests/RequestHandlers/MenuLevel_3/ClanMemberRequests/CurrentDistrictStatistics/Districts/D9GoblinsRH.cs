using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class D9GoblinsRH : BaseRequestHandler
{
    public D9GoblinsRH()
    {
        Header = "Гоблинские шахты";
        HandlerMenuLevel = MenuLevel.CurrentDistrictStatistics3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.DistrictType = ADistrictType.Goblin_Mines;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}