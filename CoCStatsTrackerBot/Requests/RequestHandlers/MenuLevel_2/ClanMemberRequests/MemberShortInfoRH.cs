using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using CoCStatsTrackerBot.Requests;

namespace CoCStatsTrackerBot.Requests;

public class MemberShortInfoRH : BaseRequestHandler
{
    public MemberShortInfoRH()
    {
        Header = "Главное об игроке";
        HandlerMenuLevel = MenuLevels.PlayerInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var member = GetFromDbQueryHandler.GetClanMember(parameters.LastTagMessage);

            var answer = PlayerFunctions.GetShortPlayerInfo(member);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
