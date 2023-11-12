﻿using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class PlayerArmyMenuHandler : BaseRequestHandler
{
    public PlayerArmyMenuHandler()
    {
        Header = "Войска";
        HandlerMenuLevel = MenuLevel.PlayerArmy3;
    }
}