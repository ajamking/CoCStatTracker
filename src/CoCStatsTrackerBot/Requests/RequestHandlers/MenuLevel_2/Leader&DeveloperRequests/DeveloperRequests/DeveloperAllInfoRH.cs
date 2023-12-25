using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class DeveloperAllInfoRH : BaseRequestHandler
{
    public DeveloperAllInfoRH()
    {
        Header = "Главное окно разработчика";
        HandlerMenuLevel = MenuLevel.DeveloperMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var answer = new StringBuilder(1000);

            answer.AppendLine(StylingHelper.MakeItStyled("Изменяемые параметры текущей сессии:\n", UiTextStyle.Header));

            answer.Append(StylingHelper.MakeItStyled("Клан, выбранный в качестве изменяемого: ", UiTextStyle.Default));

            answer.AppendLine(CheckAndGetPropertyString(parameters.LastClanTagToMerge));

            answer.Append(StylingHelper.MakeItStyled("\nНовый токен клана для вставки: ", UiTextStyle.Default));

            answer.AppendLine(CheckAndGetPropertyString(parameters.AdminKeyToMerge));

            answer.Append(StylingHelper.MakeItStyled("\nНовый тег клана для добавления: ", UiTextStyle.Default));

            answer.AppendLine(CheckAndGetPropertyString(parameters.TagToAddClan));

            answer.Append(StylingHelper.MakeItStyled("\nНовый ChatId клана для добавления: ", UiTextStyle.Default));

            answer.AppendLine(CheckAndGetPropertyString(parameters.ClanChatIdToMerge));

            answer.AppendLine(StylingHelper.MakeItStyled("\n💠💠💠💠💠💠💠💠💠💠💠💠💠💠\n", UiTextStyle.Default));
            
            answer.AppendLine(StylingHelper.MakeItStyled("Кланы, которые вы можете модерировать:", UiTextStyle.Header));

            foreach (var clan in GetFromDbQueryHandler.GetAllTrackedClans())
            {
                var isInBlackListText = $"Подписка {BeautyIcons.RedCircleEmoji}";

                if (!clan.IsInBlackList)
                {
                    isInBlackListText = $"Подписка {BeautyIcons.GreenCircleEmoji}";
                }

                var haveChatIdText = "ChatId не определен";

                if (!string.IsNullOrEmpty(clan.ClansTelegramChatId))
                {
                    haveChatIdText = clan.ClansTelegramChatId;
                }

                var newsLetterOnText = $"Рассылка {BeautyIcons.RedCircleEmoji}";

                if (!clan.RegularNewsLetterOn)
                {
                    newsLetterOnText = $"Рассылка {BeautyIcons.GreenCircleEmoji}";
                }

                answer.AppendLine(StylingHelper.MakeItStyled($"\n[{clan.Name}] - [{clan.Tag}]" +
                    $"\n[ {isInBlackListText} ] - [ {clan.AdminsKey} ]" +
                    $"\n[ {newsLetterOnText} ] - [ {haveChatIdText} ]", UiTextStyle.Name));
            }

            answer.AppendLine(StylingHelper.MakeItStyled("\n💠💠💠💠💠💠💠💠💠💠💠💠💠💠\n", UiTextStyle.Default));

            answer.AppendLine(StylingHelper.MakeItStyled("Подсказки:", UiTextStyle.Header));

            answer.AppendLine(StylingHelper.MakeItStyled("Для переопределения модерируемого клана введите один из тегов, представленных выше.\n\n" +
                "Для переопределения нового токена клана для вставки введите что-нибудь, начинаюшееся с $\n\n" +
                "Для переопределения ChatId клана введите chatId канала, добавив в начало строки символ *", UiTextStyle.Default));

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer.ToString()));
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

    private static string CheckAndGetPropertyString(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return StylingHelper.MakeItStyled("Не выбран", UiTextStyle.Name);
        }
        else
        {
            return StylingHelper.MakeItStyled(str, UiTextStyle.Name);
        }
    }
}