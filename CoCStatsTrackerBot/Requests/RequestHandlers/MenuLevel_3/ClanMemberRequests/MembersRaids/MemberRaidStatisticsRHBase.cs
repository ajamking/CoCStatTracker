using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class MemberRaidStatisticsRHBase : BaseRequestHandler
{
    public MemberRaidStatisticsRHBase()
    {
        Header = "Вызывается только через другие методы";
        HandlerMenuLevel = MenuLevel.PlayerRaidStatistics3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var raidMembershipsUi = GetFromDbQueryHandler.GetAllMemberRaidMembershipsUi(parameters.LastMemberTagMessage);

            var answer = PlayerFunctions.GetRaidStatistics(raidMembershipsUi, parameters.EntriesCount, MessageSplitToken);

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
