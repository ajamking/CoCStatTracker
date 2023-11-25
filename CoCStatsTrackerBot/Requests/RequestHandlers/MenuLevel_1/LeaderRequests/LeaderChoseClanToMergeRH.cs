using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;
using CoCStatsTrackerBot.Requests;
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
            var answer = new StringBuilder(StylingHelper.MakeItStyled("Клан, выбранный в качестве изменяемого: ", UiTextStyle.Subtitle));

            if (string.IsNullOrEmpty(parameters.LastClanTagToMerge))
            {
                answer.Append(StylingHelper.MakeItStyled("Не выбран", UiTextStyle.Name));
            }
            else
            {
                answer.Append(StylingHelper.MakeItStyled(parameters.LastClanTagToMerge, UiTextStyle.Name));
            }

            answer.AppendLine(StylingHelper.MakeItStyled("\n\nКланы, которые вы можете модерировать:\n", UiTextStyle.Subtitle));

            answer.Append(AdminsMessageHelper.GetTrackedClansStatementsMessage(parameters.IsBotHolder, parameters.AdminsKey));

            answer.AppendLine(StylingHelper.MakeItStyled("Для переопределения модерируемого клана введите один из тегов, представленных выше.", UiTextStyle.Subtitle));

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
}