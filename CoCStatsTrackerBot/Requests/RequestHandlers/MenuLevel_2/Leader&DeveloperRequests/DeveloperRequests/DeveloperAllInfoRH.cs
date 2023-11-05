using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class DeveloperAllInfoRH : BaseRequestHandler
{
    public DeveloperAllInfoRH()
    {
        Header = "Все кланы";
        HandlerMenuLevel = MenuLevel.DeveloperMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var answer = new StringBuilder(StylingHelper.MakeItStyled("Клан, выбранный в качестве изменяемого: ", UiTextStyle.Default));

            answer.Append(CheckAndGetPropertyString(parameters.LastClanTagToMerge));

            answer.Append(StylingHelper.MakeItStyled("\n\nНовый токен клана для вставки: ", UiTextStyle.Default));

            answer.Append(CheckAndGetPropertyString(parameters.AdminKeyToMerge));

            answer.Append(StylingHelper.MakeItStyled("\n\nНовый тег клана для добавления: ", UiTextStyle.Default));

            answer.Append(CheckAndGetPropertyString(parameters.TagToAddClan));

            answer.AppendLine(StylingHelper.MakeItStyled("\n\nКланы, которые вы можете модерировать: ", UiTextStyle.Default));

            foreach (var clan in GetFromDbQueryHandler.GetAllTrackedClans())
            {
                answer.AppendLine(StylingHelper.MakeItStyled($"{clan.Name} - {clan.Tag} - {clan.AdminsKey}", UiTextStyle.Name));
            }

            answer.AppendLine(StylingHelper.MakeItStyled("\nДля переопределения модерируемого клана введите один из тегов, представленных выше. ", UiTextStyle.Default));

            answer.AppendLine(StylingHelper.MakeItStyled("\nДля переопределения нового токена клана для вставки введите что-нибудь, начинаюшееся с @", UiTextStyle.Default));

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