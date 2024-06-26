﻿using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class DeveloperSetClanTokenRH : BaseRequestHandler
{
    public DeveloperSetClanTokenRH()
    {
        Header = "Установить клану токен";
        HandlerMenuLevel = MenuLevel.DeveloperMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            if (string.IsNullOrEmpty(parameters.LastClanTagToMerge) || string.IsNullOrEmpty(parameters.AdminKeyToMerge))
            {
                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Тег изменяемого клана или токен главы не проставлены.", UiTextStyle.Default));
            }
            else
            {
                UpdateDbCommandHandler.ResetClanAdminKey(parameters.LastClanTagToMerge, parameters.AdminKeyToMerge);

                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Токен для клана успешно переопределен.", UiTextStyle.Default));
            }
        }
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, DefaultNotFoundMessage);
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}