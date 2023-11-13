using CoCStatsTrackerBot.Menu;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class OtherSendFeedBackRH : BaseRequestHandler
{
    public OtherSendFeedBackRH()
    {
        Header = "Оставить отзыв";
        HandlerMenuLevel = MenuLevel.Other1;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        var answer = new StringBuilder(300);

        answer.Append(StylingHelper.MakeItStyled("Для отправки отзыва введите любое сообщение, начинающеся со слова ", UiTextStyle.Header));

        answer.AppendLine(StylingHelper.MakeItStyled(" ﴾ Отзыв ﴿\n", UiTextStyle.Name));

        answer.AppendLine(StylingHelper.MakeItStyled("Пример:\n", UiTextStyle.TableAnnotation));

        answer.AppendLine(StylingHelper.MakeItStyled("Отзыв - я крайне доволен обширным функционалом бота, мне стало проще управлять кланом " +
            "а соревновательный дух игроков поднялся до небывалых высот! Пожалание - можно ли добавить побольше ссылок на планировки?", UiTextStyle.Subtitle));

        answer.Append(StylingHelper.MakeItStyled("\nОтзыв, отправленный через бота, будет доступен только администратору.\n\nОбщедоступный отзыв можно оставить здесь: ", UiTextStyle.Default));

        answer.AppendLine(StylingHelper.GetInlineLink("Тема для отзывов", "https://t.me/CoC_Stats_Tracker_Bot_Community/179"));

        ResponseSender.SendAnswer(parameters, true, answer.ToString());
    }
}