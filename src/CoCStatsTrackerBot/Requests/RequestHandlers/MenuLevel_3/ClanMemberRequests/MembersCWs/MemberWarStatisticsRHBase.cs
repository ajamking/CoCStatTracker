using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;

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
            var cwCwlMembershipsUi = GetFromDbQueryHandler.GetAllWarMembershipsUi(parameters.LastMemberTagMessage);

            var answer = PlayerFunctions.GetWarStatistics(cwCwlMembershipsUi, parameters.EntriesCount, MessageSplitToken);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, DefaultNotFoundMessage);
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}