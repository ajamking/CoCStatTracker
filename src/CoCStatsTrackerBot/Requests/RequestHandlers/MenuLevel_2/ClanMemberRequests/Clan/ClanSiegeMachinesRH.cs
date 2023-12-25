using CoCStatsTracker;
using CoCStatsTracker.UIEntities;
using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class ClanSiegeMachinesRH : BaseRequestHandler
{
    public ClanSiegeMachinesRH()
    {
        Header = "Осадные машины";
        HandlerMenuLevel = MenuLevel.ClanInfo2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var clan = GetFromDbQueryHandler.GetTrackedClanUi(parameters.LastClanTagMessage);

            var armys = new List<ArmyUi>();

            foreach (var member in clan.ClanMembers)
            {
                armys.Add(GetFromDbQueryHandler.GetMembersArmyUi(member.Tag));
            }

            var answer = ClanFunctions.GetClanSiegeMachines(armys);

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