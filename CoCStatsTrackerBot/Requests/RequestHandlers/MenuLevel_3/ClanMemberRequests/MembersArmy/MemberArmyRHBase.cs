using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class MemberArmyRHBase : BaseRequestHandler
{
    public MemberArmyRHBase()
    {
        Header = "Вызывается только через другие методы";
        HandlerMenuLevel = MenuLevels.PlayerArmy3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var playerArmy = GetFromDbQueryHandler.GetMembersArmy(parameters.LastTagMessage);

            var answer = PlayerFunctions.GetMembersArmyInfo(playerArmy, parameters.UnitType);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
