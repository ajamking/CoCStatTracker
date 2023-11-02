using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using CoCStatsTrackerBot.Requests;

namespace CoCStatsTrackerBot.Requests;

public class ClanShortInfoRH : BaseRequestHandler
{
    public ClanShortInfoRH()
    {
        Header = "Главное о клане";
        HandlerMenuLevel = MenuLevel.ClanInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var clan = GetFromDbQueryHandler.GetTrackedClan(parameters.LastClanTagMessage);

            var answer = ClanFunctions.GetClanShortInfo(clan);

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
