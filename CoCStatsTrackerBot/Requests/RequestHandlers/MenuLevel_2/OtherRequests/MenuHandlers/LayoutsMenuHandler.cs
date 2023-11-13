﻿using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class LayoutsMenuHandler : BaseRequestHandler
{
    public LayoutsMenuHandler()
    {
        Header = "Планировки";
        HandlerMenuLevel = MenuLevel.Layouts2;
    }
}