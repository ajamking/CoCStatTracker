using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class LeaderChoseClanToMergeRH : BaseRequestHandler
{
    public LeaderChoseClanToMergeRH()
    {
        Header = "Доступные кланы";
        HandlerMenuLevel = MenuLevel.Leader1;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var answer = new StringBuilder(StylingHelper.MakeItStyled("Клан, выбранный в качестве изменяемого: ", UiTextStyle.Default));

            if (string.IsNullOrEmpty(parameters.LastClanTagToMerge))
            {
                answer.Append(StylingHelper.MakeItStyled("Не выбран", UiTextStyle.Name));
            }
            else
            {
                answer.Append(StylingHelper.MakeItStyled(parameters.LastClanTagToMerge, UiTextStyle.Name));
            }

            answer.AppendLine(StylingHelper.MakeItStyled("\n\nКланы, которые вы можете модерировать: ", UiTextStyle.Default));

            foreach (var clan in GetFromDbQueryHandler.GetAllTrackedClans().Where(x => x.AdminsKey == parameters.AdminsKey))
            {
                answer.AppendLine(StylingHelper.MakeItStyled($"{clan.Name} - {clan.Tag}", UiTextStyle.Name));
            }

            answer.AppendLine(StylingHelper.MakeItStyled("\nДля переопределения модерируемого клана введите один из тегов, представленных выше. ", UiTextStyle.Default));

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
}
