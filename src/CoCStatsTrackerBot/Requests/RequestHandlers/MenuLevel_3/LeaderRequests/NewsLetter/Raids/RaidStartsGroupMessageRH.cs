﻿using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class RaidStartsGroupMessageRH : BaseRequestHandler
{
    public RaidStartsGroupMessageRH()
    {
        Header = $"Начало рейдов {BeautyIcons.RedCircleEmoji}/{BeautyIcons.GreenCircleEmoji}";
        HandlerMenuLevel = MenuLevel.LeaderNewsLetterCustomize3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var answer = new StringBuilder(500);

            if (!string.IsNullOrEmpty(parameters.LastClanTagToMerge))
            {
                var clanDb = GetFromDbQueryHandler.GetTrackedClan(parameters.LastClanTagToMerge);

                UpdateDbCommandHandler.ResetClanRegularNewsLetter(clanDb.Tag, NewsLetterType.RaidStart);

                var tempAnswer = StylingHelper.MakeItStyled($"Операция успешна. Рассылка о начале рейдов {(clanDb.RaidStartMessageOn ? "вылючена." : "включена.")}\n", UiTextStyle.Subtitle);

                ResponseSender.SendAnswer(parameters, true, $"{tempAnswer}\n{AdminsMessageHelper.GetOneTrackedClanStatement(clanDb.Tag)}");
            }
            else
            {
                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Для использования этой функции необходимо выбрать модерируемый клан.", UiTextStyle.Default));
            }
        }
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Либо такой клан не отслеживается, либо в нем нет участников.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}