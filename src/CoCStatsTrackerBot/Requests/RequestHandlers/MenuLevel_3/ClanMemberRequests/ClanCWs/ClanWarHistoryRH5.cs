﻿using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class ClanWarHistoryRH5 : BaseRequestHandler
{
    public ClanWarHistoryRH5()
    {
        Header = "Последние 5";
        HandlerMenuLevel = MenuLevel.ClanWarsHistory3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 5;

        var handler = new ClanWarHistoryRHBase();

        handler.Execute(parameters);
    }
}