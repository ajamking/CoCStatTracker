using CoCStatsTrackerBot.BotMenues;
using Domain.Entities;

namespace CoCStatsTrackerBot.Requests;

public class MemberEveryUnitInfoRH : BaseRequestHandler
{
    public MemberEveryUnitInfoRH()
    {
        Header = "Обычные войска";
        HandlerMenuLevel = MenuLevel.PlayerArmy3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.UnitType = UnitType.EveryUnit;

        var handler = new MemberArmyRHBase();

        handler.Execute(parameters);
    }
}