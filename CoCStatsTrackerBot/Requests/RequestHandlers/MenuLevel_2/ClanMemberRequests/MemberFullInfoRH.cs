using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using CoCStatsTrackerBot.Requests;

namespace CoCStatsTrackerBot.Requests;

public class MemberFullInfoRH : BaseRequestHandler
{
    public MemberFullInfoRH()
    {
        Header = "Все об игроке";
        HandlerMenuLevel = MenuLevels.PlayerInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var member = GetFromDbQueryHandler.GetClanMember(parameters.LastTagMessage);

            var answer = Requests.PlayerFunctions.GetFullPlayerInfo(member);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
