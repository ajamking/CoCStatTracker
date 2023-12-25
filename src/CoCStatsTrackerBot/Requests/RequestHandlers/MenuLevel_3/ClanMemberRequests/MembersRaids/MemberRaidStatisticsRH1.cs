using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class MemberRaidStatisticsRH1 : BaseRequestHandler
{
    public MemberRaidStatisticsRH1()
    {
        Header = "Последнее участие в рейдах";
        HandlerMenuLevel = MenuLevel.PlayerRaidStatistics3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 1;

        var handler = new MemberRaidStatisticsRHBase();

        handler.Execute(parameters);
    }
}