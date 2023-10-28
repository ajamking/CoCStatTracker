using CoCStatsTracker;
using CoCStatsTracker.UIEntities;
using CoCStatsTrackerBot.Menu;
using CoCStatsTrackerBot.Requests;

namespace CoCStatsTrackerBot.Requests;

public class ClanSiegeMachinesRH : BaseRequestHandler
{
    public ClanSiegeMachinesRH()
    {
        Header = "Осадные машины";
        HandlerMenuLevel = MenuLevels.ClanInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var clan = GetFromDbQueryHandler.GetTrackedClan(parameters.LastTagMessage);

            var armys = new List<ArmyUi>();

            foreach (var member in clan.ClanMembers)
            {
                armys.Add(GetFromDbQueryHandler.GetMembersArmy(member.Tag));
            }

            var answer = ClanFunctions.GetClanSiegeMachines(armys);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
