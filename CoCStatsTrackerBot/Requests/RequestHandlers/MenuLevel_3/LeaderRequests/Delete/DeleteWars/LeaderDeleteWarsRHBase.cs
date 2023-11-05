﻿using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteWarsRHBase : BaseRequestHandler
{
    public LeaderDeleteWarsRHBase()
    {
        Header = "Вызывается только через другие методы";
        HandlerMenuLevel = MenuLevel.LeaderDeleteRaidsMenu3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var clanWars = GetFromDbQueryHandler.GetAllClanWars(parameters.LastClanTagToMerge).Count;

            DeleteFromDbCommandHandler.DeleteClanWars(parameters.LastClanTagToMerge, parameters.EntriesCount);

            var clanWarsAfterRemove = GetFromDbQueryHandler.GetAllRaids(parameters.LastClanTagToMerge).Count;

            var answer = StylingHelper.MakeItStyled($"Операция успешна.\n" +
                $"Записей удалено: {clanWars - clanWarsAfterRemove}", UiTextStyle.Default);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Все записи о войнах удалены.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}