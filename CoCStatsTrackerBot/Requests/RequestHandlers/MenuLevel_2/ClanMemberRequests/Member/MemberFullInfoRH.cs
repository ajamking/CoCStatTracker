using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class MemberFullInfoRH : BaseRequestHandler
{
    public MemberFullInfoRH()
    {
        Header = "Все об игроке";
        HandlerMenuLevel = MenuLevel.PlayerInfo2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var member = GetFromDbQueryHandler.GetClanMemberUi(parameters.LastMemberTagMessage);

            var answer = PlayerFunctions.GetFullPlayerInfo(member);

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
