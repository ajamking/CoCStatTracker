using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class MemberWarStatisticsRH1 : BaseRequestHandler
{
    public MemberWarStatisticsRH1()
    {
        Header = "Последняя война";
        HandlerMenuLevel = MenuLevels.PlayerWarStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.EntriesCount = 1;

        var handler = new MemberWarStatisticsRHBase();

        handler.Execute(parameters);
    }
}
