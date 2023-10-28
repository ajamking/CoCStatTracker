using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class MemberRaidStatisticsRH1 : BaseRequestHandler
{
    public MemberRaidStatisticsRH1()
    {
        Header = "Последний рейд";
        HandlerMenuLevel = MenuLevels.PlayerRaidStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.EntriesCount = 1;

        var handler = new MemberRaidStatisticsRHBase();

        handler.Execute(parameters);
    }
}
