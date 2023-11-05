using CoCStatsTracker;
using CoCStatsTracker.UIEntities;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanActiveSuperUnitsRH : BaseRequestHandler
{
    public ClanActiveSuperUnitsRH()
    {
        Header = "Активные супер юниты";
        HandlerMenuLevel = MenuLevel.ClanInfo2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var clan = GetFromDbQueryHandler.GetTrackedClan(parameters.LastClanTagMessage);

            var armys = new List<ArmyUi>();

            foreach (var member in clan.ClanMembers)
            {
                armys.Add(GetFromDbQueryHandler.GetMembersArmy(member.Tag));
            }

            var answer = ClanFunctions.GetClanActiveeSuperUnits(armys);

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
