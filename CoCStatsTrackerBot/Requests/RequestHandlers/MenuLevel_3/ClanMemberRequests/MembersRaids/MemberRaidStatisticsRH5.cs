using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class MemberRaidStatisticsRH5 : BaseRequestHandler
{
    public MemberRaidStatisticsRH5()
    {
        Header = "Последние 5";
        HandlerMenuLevel = MenuLevels.PlayerRaidStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.EntriesCount = 5;

        var handler = new MemberRaidStatisticsRHBase();

        handler.Execute(parameters);
    }
}
