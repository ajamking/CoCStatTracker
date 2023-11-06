using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanAllMembersRH : BaseRequestHandler
{
    public ClanAllMembersRH()
    {
        Header = "Члены клана";
        HandlerMenuLevel = MenuLevel.ClanInfo2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var clanMembers = GetFromDbQueryHandler.GetAllClanMembersUi(parameters.LastClanTagMessage);

            var answer = ClanFunctions.GetClanMembers(clanMembers);

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
