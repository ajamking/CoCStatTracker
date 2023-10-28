using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanAllMembersRH : BaseRequestHandler
{
    public ClanAllMembersRH()
    {
        Header = "Члены клана";
        HandlerMenuLevel = MenuLevels.ClanInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var clanMembers = GetFromDbQueryHandler.GetAllClanMembers(parameters.LastTagMessage);

            var answer = ClanFunctions.GetClanMembers(clanMembers);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
