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

            var trackedClans = GetFromDbQueryHandler.GetAllTrackedClansUi();

            if (parameters.IsBotHolder)
            {
                foreach (var clan in trackedClans)
                {
                    answer.AppendLine(StylingHelper.MakeItStyled($"{clan.Name} - {clan.Tag}", UiTextStyle.Name));
                }
            }
            else
            {
                foreach (var clan in trackedClans.Where(x => x.AdminsKey == parameters.AdminsKey).Where(x => x.IsInBlackList == false))
                {
                    answer.AppendLine(StylingHelper.MakeItStyled($"{clan.Name} - {clan.Tag}", UiTextStyle.Name));
                }
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
