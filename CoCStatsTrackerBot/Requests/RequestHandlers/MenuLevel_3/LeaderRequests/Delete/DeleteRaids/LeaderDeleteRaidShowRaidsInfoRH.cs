using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteRaidShowRaidsInfoRH : BaseRequestHandler
{
    public LeaderDeleteRaidShowRaidsInfoRH()
    {
        Header = "Зафиксированные рейды.";
        HandlerMenuLevel = MenuLevel.LeaderDeleteRaidsMenu3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var raids = GetFromDbQueryHandler.GetAllRaidsUi(parameters.LastClanTagToMerge);

            var answer = new StringBuilder(StylingHelper.MakeItStyled($"Зафиксированные рейды клана {parameters.LastClanTagToMerge} :\n\n", UiTextStyle.Header));

            var count = 1;

            foreach (var raid in raids)
            {
                answer.AppendLine(StylingHelper.MakeItStyled($"{count}. {raid.StartedOn.ToShortDateString()} - {raid.EndedOn.ToShortDateString()} - {raid.State}", UiTextStyle.Default));

                count++;
            }

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer.ToString()));
        }
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Все записи о рейдах удалены или не отслеживались вовсе.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}