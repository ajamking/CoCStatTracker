using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteWarsShowWarsInfoRH : BaseRequestHandler
{
    public LeaderDeleteWarsShowWarsInfoRH()
    {
        Header = "Зафиксированные войны";
        HandlerMenuLevel = MenuLevel.LeaderDeleteClanWarsMenu3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var wars = GetFromDbQueryHandler.GetAllClanWarsUi(parameters.LastClanTagToMerge);

            var answer = new StringBuilder(StylingHelper.MakeItStyled($"Зафиксированные войны клана {parameters.LastClanTagToMerge} :\n\n", UiTextStyle.Header));

            if (wars.Count is not 0)
            {
                var count = 1;

                foreach (var war in wars)
                {
                    answer.AppendLine(StylingHelper.MakeItStyled($"{count}. {war.StartedOn.ToShortDateString()} - {war.EndedOn.ToShortDateString()} - {war.Result}", UiTextStyle.Default));

                    count++;
                }
            }
            else
            {
                answer.AppendLine(StylingHelper.MakeItStyled("Записи отсутствуют", UiTextStyle.Default));
            }

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer.ToString()));
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Все записи о войнах удалены или не отслеживались вовсе.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}