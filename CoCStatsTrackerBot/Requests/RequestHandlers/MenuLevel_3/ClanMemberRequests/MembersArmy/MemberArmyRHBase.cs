using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class MemberArmyRHBase : BaseRequestHandler
{
    public MemberArmyRHBase()
    {
        Header = "Вызывается только через другие методы";
        HandlerMenuLevel = MenuLevel.PlayerArmy3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var playerArmy = GetFromDbQueryHandler.GetMembersArmy(parameters.LastMemberTagMessage);

            var answer = PlayerFunctions.GetMembersArmyInfo(playerArmy, parameters.UnitType);

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
