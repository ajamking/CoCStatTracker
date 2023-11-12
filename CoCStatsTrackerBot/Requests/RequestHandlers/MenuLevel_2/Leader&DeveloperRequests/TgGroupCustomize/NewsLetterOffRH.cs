﻿using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class NewsLetterOffRH : BaseRequestHandler
{
    public NewsLetterOffRH()
    {
        Header = "Выключить рассылку";
        HandlerMenuLevel = MenuLevel.LeaderTgGroupCustomize2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var answer = new StringBuilder(500);

            if (!string.IsNullOrEmpty(parameters.LastClanTagToMerge))
            {
                UpdateDbCommandHandler.ResetRegularNewsLetter(parameters.LastClanTagToMerge, false);

                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Рассылка выключена.", UiTextStyle.Default));
            }
            else
            {
                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Для использования этой функции необходимо выбрать модерируемый клан.", UiTextStyle.Default));
            }
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Либо такой клан не отслеживается, либо в нем нет участников.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}