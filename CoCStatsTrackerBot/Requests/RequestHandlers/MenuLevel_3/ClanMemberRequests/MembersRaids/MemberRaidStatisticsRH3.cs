using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class MemberRaidStatisticsRH3 : BaseRequestHandler
{
    public MemberRaidStatisticsRH3()
    {
        Header = "3 последних рейда";
        HandlerMenuLevel = MenuLevel.PlayerRaidStatistics3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 3;

        var handler = new MemberRaidStatisticsRHBase();

        handler.Execute(parameters);
    }
}