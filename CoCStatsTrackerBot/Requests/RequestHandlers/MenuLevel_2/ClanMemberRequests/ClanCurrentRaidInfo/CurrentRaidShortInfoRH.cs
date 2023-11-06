using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class CurrentRaidShortInfoRH : BaseRequestHandler
{
    public CurrentRaidShortInfoRH()
    {
        Header = "Главное о рейде";
        HandlerMenuLevel = MenuLevel.CurrentRaidInfo2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var allRaids = GetFromDbQueryHandler.GetAllRaidsUi(parameters.LastClanTagMessage).OrderByDescending(x => x.StartedOn);

            var answer = CurrentStatisticsFunctions.GetCurrentRaidShortInfo(allRaids.First());

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
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
