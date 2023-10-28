using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class MemberWarStatisticsRHBase : BaseRequestHandler
{
    public MemberWarStatisticsRHBase()
    {
        Header = "Вызывается только через другие методы";
        HandlerMenuLevel = MenuLevels.PlayerWarStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var cwCwlMembershipsUi = GetFromDbQueryHandler.GetAllMemberСwCwlMemberships(parameters.LastTagMessage);

            var answer = PlayerFunctions.GetWarStatistics(cwCwlMembershipsUi, parameters.EntriesCount, MessageSplitToken);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
