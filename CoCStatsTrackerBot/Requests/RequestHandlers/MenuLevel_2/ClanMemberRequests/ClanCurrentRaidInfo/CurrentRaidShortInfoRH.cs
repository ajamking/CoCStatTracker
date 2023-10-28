using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using CoCStatsTrackerBot.Requests;

namespace CoCStatsTrackerBot.Requests;

public class CurrentRaidShortInfoRH : BaseRequestHandler
{
    public CurrentRaidShortInfoRH()
    {
        Header = "Главное";
        HandlerMenuLevel = MenuLevels.CurrentRaidInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var allRaids = GetFromDbQueryHandler.GetAllRaids(parameters.LastTagMessage).OrderByDescending(x => x.StartedOn);

            var answer = CurrentStatisticsFunctions.GetCurrentRaidShortInfo(allRaids.First());

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
