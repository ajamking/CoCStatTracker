using CoCStatsTrackerBot.Menu;
using Domain.Entities;

namespace CoCStatsTrackerBot.Requests;

public class MemberSuperUnitsRH : BaseRequestHandler
{
    public MemberSuperUnitsRH()
    {
        Header = "Активные супер юниты игрока";
        HandlerMenuLevel = MenuLevel.PlayerArmy3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.UnitType = UnitType.SuperUnit;

        var handler = new MemberArmyRHBase();

        handler.Execute(parameters);
    }
}
