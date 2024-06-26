﻿using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class LeaderAddLastRaidRH : BaseRequestHandler
{
    public LeaderAddLastRaidRH()
    {
        Header = "Добавить последний рейд";
        HandlerMenuLevel = MenuLevel.LeaderAddMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            AddToDbCommandHandler.AddCurrentRaidToClan(parameters.LastClanTagToMerge);

            var lastRaid = GetFromDbQueryHandler.GetLastRaidUi(parameters.LastClanTagToMerge);

            var answer = StylingHelper.MakeItStyled($"Операция успешна.\n" +
                $"Добавлен рейд: {lastRaid.StartedOn.ToShortDateString()} - {lastRaid.EndedOn.ToShortTimeString()} {lastRaid.State}", UiTextStyle.Default);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, DefaultNotFoundMessage);
        }
        catch (AlreadyExistsException)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Последний рейд уже отслеживается, добавить его невозможно, но можно обновить или удалить в других вкладках.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}