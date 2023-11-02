using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class MemberWarStatisticsRH3 : BaseRequestHandler
{
    public MemberWarStatisticsRH3()
    {
        Header = "3 последних войны";
        HandlerMenuLevel = MenuLevel.PlayerWarStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.EntriesCount = 3;

        var handler = new MemberWarStatisticsRHBase();

        handler.Execute(parameters);
    }
}
