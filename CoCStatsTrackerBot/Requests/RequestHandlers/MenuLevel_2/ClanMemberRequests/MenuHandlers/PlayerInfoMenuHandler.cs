﻿using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class PlayerInfoMenuHandler : BaseRequestHandler
{
    public PlayerInfoMenuHandler()
    {
        Header = "Игрок";
        HandlerMenuLevel = MenuLevel.PlayerInfo2;
    }
}
