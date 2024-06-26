﻿using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class ClanWarHistoryRH3 : BaseRequestHandler
{
    public ClanWarHistoryRH3()
    {
        Header = "Последние 3";
        HandlerMenuLevel = MenuLevel.ClanWarsHistory3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 3;

        var handler = new ClanWarHistoryRHBase();

        handler.Execute(parameters);
    }
}