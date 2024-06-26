﻿using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class D3WizzardsRH : BaseRequestHandler
{
    public D3WizzardsRH()
    {
        Header = "Долина колдунов";
        HandlerMenuLevel = MenuLevel.CurrentDistrictStatistics3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.DistrictType = DistrictType.Wizard_Valley;

        var handler = new CurrentDistrictStatisticsRHBase();

        handler.Execute(parameters);
    }
}