﻿using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class ClanInfoMenuHandler : BaseRequestHandler
{
    public ClanInfoMenuHandler()
    {
        Header = "Клан";
        HandlerMenuLevel = MenuLevel.ClanInfo2;
    }
}