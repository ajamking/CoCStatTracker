using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using CoCStatsTrackerBot.Requests;

namespace CoCStatsTrackerBot.Requests;

public class CurrentClanWarShortInfoRH : BaseRequestHandler
{
    public CurrentClanWarShortInfoRH()
    {
        Header = "Главное";
        HandlerMenuLevel = MenuLevels.CurrentWarInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var allClanWars = GetFromDbQueryHandler.GetAllClanWars(parameters.LastTagMessage).OrderByDescending(x => x.StartedOn);

            var answer = CurrentStatisticsFunctions.GetCurrentWarShortInfo(allClanWars.First());

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
