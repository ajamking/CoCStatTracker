using CoCStatsTrackerBot.Menu;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot.Requests;

public class OtherStickersRH : BaseRequestHandler
{
    public OtherStickersRH()
    {
        Header = "CoC Стикеры";
        HandlerMenuLevel = MenuLevel.Other1;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        var answer = new StringBuilder(StylingHelper.MakeItStyled("Cтикерпаки по Clash of Clans\n", UiTextStyle.Header));

        answer.AppendLine(StylingHelper.GetInlineLink("Стикерпак Killers Empire Family", "https://t.me/addstickers/spe4f49a4a82940c3317877cafbb0c42a5_by_stckrRobot"));

        answer.AppendLine();

        answer.AppendLine(StylingHelper.GetInlineLink("Анимированный стикерпак по вселенной Clash", "https://t.me/addstickers/pack751_by_Stick4uBot"));

        answer.AppendLine(StylingHelper.MakeItStyled("\nЕсли и у вас есть свои достойные стикерпаки по клешу и вы хотели бы поделиться ими с комьюнити - " +
            "предложите это администратору.", UiTextStyle.Default));

        ResponseSender.SendAnswer(parameters, true, answer.ToString());

        parameters.BotClient.SendStickerAsync(parameters.Message.Chat.Id,
                       sticker: "CAACAgIAAxkBAAJxh2VSOIgO459G_5-YeFLT2zvT7mU3AAKhOAACNcSZSucZC7zWC2ehMwQ");
    }
}