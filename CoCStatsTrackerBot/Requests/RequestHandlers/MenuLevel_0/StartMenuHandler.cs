using CoCStatsTrackerBot.BotMenues;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class StartMenuHandler : BaseRequestHandler
{
    public StartMenuHandler()
    {
        Header = "/start";
        HandlerMenuLevel = MenuLevel.Main0;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        var answer = new StringBuilder(StylingHelper.MakeItStyled("Добро пожаловать!\n\n", UiTextStyle.Header));

        answer.AppendLine(StylingHelper.MakeItStyled("Я - уникальный бот для сбора всевозможной статистики вашего клана в игре Clash of Clans\n", UiTextStyle.Name));

        answer.AppendLine(StylingHelper.GetInlineLink("Группа пользователей бота", "https://t.me/CoC_Stats_Tracker_Bot_Community"));

        answer.AppendLine(StylingHelper.GetInlineLink("\nАдминистратор", "https://t.me/StatsTrackerAdmin"));

        answer.AppendLine(StylingHelper.GetInlineLink("\nРуководство по использованию", "https://disk.yandex.ru/d/ZcPoSyQsDSPImA"));

        answer.AppendLine(StylingHelper.MakeItStyled("\nКраткое описание:\n", UiTextStyle.TableAnnotation));

        answer.AppendLine(StylingHelper.MakeItStyled("1. Бот предназначен для сбора и визуализации статиситики только отслеживаемых кланов.\n", UiTextStyle.Default));

        answer.AppendLine(StylingHelper.MakeItStyled("2. Все взаимодействие с ботом осуществляется через кнопочное меню (кнопка раскрытия меню " +
            "находится в правой нижней части экрана), меню в левой нижней части - команды для групповых чатов.\n", UiTextStyle.Default));

        answer.AppendLine(StylingHelper.MakeItStyled("3. Бот находится в состоянии Бета-теста, иногда может не работать или крашиться.\n", UiTextStyle.Default));

        answer.AppendLine(StylingHelper.MakeItStyled("4. Даты и время, выводимые ботом, соответствют часовому поясу UTC+3 (московское время).\n", UiTextStyle.Default));

        ResponseSender.SendAnswer(parameters, true, answer.ToString());
    }
}