using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanSeasonalStatisticRH : BaseRequestHandler
{
    public ClanSeasonalStatisticRH()
    {
        Header = "Показатели месяца";
        HandlerMenuLevel = MenuLevel.ClanInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var seasonalStatistics = GetFromDbQueryHandler.GetSeasonStatistics(parameters.LastClanTagMessage);

            var answer = ClanFunctions.GetSeasonClanMembersStatistcs(seasonalStatistics);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Пока не обладаю такими сведениями.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
