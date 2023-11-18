using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class MemberWarStatisticsRH5 : BaseRequestHandler
{
    public MemberWarStatisticsRH5()
    {
        Header = "5 последних войн";
        HandlerMenuLevel = MenuLevel.PlayerWarStatistics3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 5;

        var handler = new MemberWarStatisticsRHBase();

        handler.Execute(parameters);
    }
}