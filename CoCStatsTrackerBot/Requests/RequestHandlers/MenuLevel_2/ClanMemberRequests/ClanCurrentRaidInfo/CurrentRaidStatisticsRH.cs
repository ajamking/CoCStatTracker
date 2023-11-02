﻿using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using CoCStatsTrackerBot.Requests;

namespace CoCStatsTrackerBot.Requests;

public class CurrentRaidStatisticsRH : BaseRequestHandler
{
    public CurrentRaidStatisticsRH()
    {
        Header = "Показатели рейда";
        HandlerMenuLevel = MenuLevel.CurrentRaidInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            parameters.EntriesCount = 1;

            var allRaids = GetFromDbQueryHandler.GetAllRaids(parameters.LastClanTagMessage).OrderByDescending(x => x.StartedOn).ToList();

            var answer = ClanFunctions.GetRaidsHistory(allRaids, parameters.EntriesCount, MessageSplitToken);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Пока не обладаю такими сведениями.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
