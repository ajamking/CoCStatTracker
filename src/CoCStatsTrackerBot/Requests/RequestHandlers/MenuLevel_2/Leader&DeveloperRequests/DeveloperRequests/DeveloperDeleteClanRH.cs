﻿using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class DeveloperDeleteClanRH : BaseRequestHandler
{
    public DeveloperDeleteClanRH()
    {
        Header = "Удалить клан из БД";
        HandlerMenuLevel = MenuLevel.DeveloperMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            if (!string.IsNullOrEmpty(parameters.LastClanTagToMerge))
            {
                DeleteFromDbCommandHandler.DeleteTrackedClan(parameters.LastClanTagToMerge);

                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Клан, успешно удален из БД.", UiTextStyle.Default));
            }
            else
            {
                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Тег для изменяемого клана не проставлен.", UiTextStyle.Default));
            }
        }
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Такого клана нет в БД, удаление невозможно.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}