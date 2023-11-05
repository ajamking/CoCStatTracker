using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using CoCStatsTrackerBot.Requests;

namespace CoCStatsTrackerBot.Requests;

public class CurrentClanWarShortInfoRH : BaseRequestHandler
{
    public CurrentClanWarShortInfoRH()
    {
        Header = "Главное о войне";
        HandlerMenuLevel = MenuLevel.CurrentWarInfo2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var allClanWars = GetFromDbQueryHandler.GetAllClanWars(parameters.LastClanTagMessage).OrderByDescending(x => x.StartedOn);

            var answer = CurrentStatisticsFunctions.GetCurrentWarShortInfo(allClanWars.First());

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
