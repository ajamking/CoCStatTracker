﻿using CoCStatsTrackerBot.Menu;
using Domain.Entities;

namespace CoCStatsTrackerBot.Requests;

public class MemberSiegeMachinesRH : BaseRequestHandler
{
    public MemberSiegeMachinesRH()
    {
        Header = "Осадные машины игрока";
        HandlerMenuLevel = MenuLevel.PlayerArmy3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.UnitType = UnitType.SiegeMachine;

        var handler = new MemberArmyRHBase();

        handler.Execute(parameters);
    }
}
