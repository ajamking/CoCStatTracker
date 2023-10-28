using CoCStatsTracker;
using CoCStatsTracker.UIEntities;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanActiveSuperUnitsRH : BaseRequestHandler
{
    public ClanActiveSuperUnitsRH()
    {
        Header = "Активные супер юниты";
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

            var answer = ClanFunctions.GetClanActiveeSuperUnits(armys);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
