using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class MemberWarStatisticsRH3 : BaseRequestHandler
{
    public MemberWarStatisticsRH3()
    {
        Header = "Последние 3";
        HandlerMenuLevel = MenuLevels.PlayerWarStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.EntriesCount = 3;

        var handler = new MemberWarStatisticsRHBase();

        handler.Execute(parameters);
    }
}
