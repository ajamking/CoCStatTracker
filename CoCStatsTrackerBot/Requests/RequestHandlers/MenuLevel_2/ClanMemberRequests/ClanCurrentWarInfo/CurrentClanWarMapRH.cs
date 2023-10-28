using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using CoCStatsTrackerBot.Requests;

namespace CoCStatsTrackerBot.Requests;

public class CurrentClanWarMapRH : BaseRequestHandler
{
    public CurrentClanWarMapRH()
    {
        Header = "Карта";
        HandlerMenuLevel = MenuLevels.CurrentWarInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var allClanWars = GetFromDbQueryHandler.GetAllClanWars(parameters.LastTagMessage).OrderByDescending(x => x.StartedOn);

            var answer = CurrentStatisticsFunctions.GetCurrentWarMap(allClanWars.First().WarMap);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
