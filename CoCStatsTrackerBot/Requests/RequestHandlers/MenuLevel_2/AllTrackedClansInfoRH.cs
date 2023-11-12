﻿using CoCStatsTracker;
using CoCStatsTrackerBot.Helpers;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class AllTrackedClansInfoRH : BaseRequestHandler
{
    public AllTrackedClansInfoRH()
    {
        Header = "Все отслеживаемые кланы";
        HandlerMenuLevel = MenuLevel.Member1;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            TagsConditionChecker.SendClanTagMessageIsEmpty(parameters);
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, DefaultNotFoundMessage);
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}