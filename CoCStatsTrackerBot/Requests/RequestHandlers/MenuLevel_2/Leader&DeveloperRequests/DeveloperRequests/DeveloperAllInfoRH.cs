using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
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

            foreach (var clan in GetFromDbQueryHandler.GetAllTrackedClansUi())
            {
                var isInBlackListText = "Подписка активна";

                if (clan.IsInBlackList)
                {
                    isInBlackListText = "Подписка приостановлена";
                }

                var haveChatIdText = "ChatId не определен";

                if (!string.IsNullOrEmpty(clan.ClanChatId))
                {
                    haveChatIdText = clan.ClanChatId;
                }

                answer.AppendLine(StylingHelper.MakeItStyled($"\n[{clan.Name}] - [{clan.Tag}] - [{clan.AdminsKey}]\n[{haveChatIdText}] - [{isInBlackListText}]", UiTextStyle.Name));
            }

            answer.AppendLine(StylingHelper.MakeItStyled("\n💠💠💠💠💠💠💠💠💠💠💠💠💠💠\n", UiTextStyle.Default));

            answer.AppendLine(StylingHelper.MakeItStyled("Подсказки:", UiTextStyle.Header));

            answer.AppendLine(StylingHelper.MakeItStyled("Для переопределения модерируемого клана введите один из тегов, представленных выше.\n\n" +
                "Для переопределения нового токена клана для вставки введите что-нибудь, начинаюшееся с $\n\n" +
                "Для переопределения ChatId клана введите chatId канала, добавив в начало строки символ *", UiTextStyle.Default));

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer.ToString()));
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, DefaultNotFoundMessage);
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }

    private string CheckAndGetPropertyString(string str)
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