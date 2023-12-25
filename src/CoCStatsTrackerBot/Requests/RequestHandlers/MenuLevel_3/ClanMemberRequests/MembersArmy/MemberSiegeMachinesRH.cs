using CoCStatsTrackerBot.BotMenues;
using Domain.Entities;

namespace CoCStatsTrackerBot.Requests;

public class MemberSiegeMachinesRH : BaseRequestHandler
{
    public MemberSiegeMachinesRH()
    {
        Header = "Осадные машины игрока";
        HandlerMenuLevel = MenuLevel.PlayerArmy3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.UnitType = UnitType.SiegeMachine;

        var handler = new MemberArmyRHBase();

        handler.Execute(parameters);
    }
}