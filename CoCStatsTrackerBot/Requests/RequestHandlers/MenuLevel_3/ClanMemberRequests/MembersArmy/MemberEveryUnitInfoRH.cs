using CoCStatsTrackerBot.Menu;
using Domain.Entities;

namespace CoCStatsTrackerBot.Requests;

public class MemberEveryUnitInfoRH : BaseRequestHandler
{
    public MemberEveryUnitInfoRH()
    {
        Header = "Все войска";
        HandlerMenuLevel = MenuLevels.PlayerArmy3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.UnitType = UnitType.EveryUnit;

        var handler = new MemberArmyRHBase();

        handler.Execute(parameters);
    }
}
