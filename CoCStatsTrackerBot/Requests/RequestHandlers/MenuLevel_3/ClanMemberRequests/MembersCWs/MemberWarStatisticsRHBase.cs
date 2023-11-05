using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class MemberWarStatisticsRHBase : BaseRequestHandler
{
    public MemberWarStatisticsRHBase()
    {
        Header = "Вызывается только через другие методы";
        HandlerMenuLevel = MenuLevel.PlayerWarStatistics3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var cwCwlMembershipsUi = GetFromDbQueryHandler.GetAllMemberСwCwlMemberships(parameters.LastMemberTagMessage);

            var answer = PlayerFunctions.GetWarStatistics(cwCwlMembershipsUi, parameters.EntriesCount, MessageSplitToken);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, DefaultNotFoundMessage);
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
