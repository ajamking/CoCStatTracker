using CoCStatsTrackerBot.BotMenues;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class OtherContactsRH : BaseRequestHandler
{
    public OtherContactsRH()
    {
        Header = "Контакты";
        HandlerMenuLevel = MenuLevel.Other1;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        var answer = new StringBuilder(StylingHelper.MakeItStyled("Контакты: \n\n", UiTextStyle.Header));

        var linksDic = new Dictionary<string, string>()
        {
            { "Сообщество пользователей бота","https://t.me/CoC_Stats_Tracker_Bot_Community" },
            { "Администратор бота","https://t.me/StatsTrackerAdmin" },
        };

        foreach (var link in linksDic)
        {
            answer.AppendLine($"{StylingHelper.GetInlineLink($"》 {link.Key}", link.Value)}\n");
        }

        answer.AppendLine(StylingHelper.MakeItStyled($"Вы можете обращаться к администратору по вопросам оформления подписки, " +
            $"предложениями по усовершенствованию бота или с вопросами по его функционалу. \nПрежде чем обращаться с последним - настоятельно " +
            $"рекомендуется прочесть руководство.", UiTextStyle.Default));

        ResponseSender.SendAnswer(parameters, true, answer.ToString());
    }
}