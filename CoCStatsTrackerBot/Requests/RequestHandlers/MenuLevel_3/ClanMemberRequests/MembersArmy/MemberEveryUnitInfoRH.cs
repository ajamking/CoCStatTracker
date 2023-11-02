using CoCStatsTrackerBot.Menu;
using Domain.Entities;

namespace CoCStatsTrackerBot.Requests;

public class MemberEveryUnitInfoRH : BaseRequestHandler
{
    public MemberEveryUnitInfoRH()
    {
        Header = "Все войска";
        HandlerMenuLevel = MenuLevel.PlayerArmy3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.UnitType = UnitType.EveryUnit;

        var handler = new MemberArmyRHBase();

        handler.Execute(parameters);
    }
}
