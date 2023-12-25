using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;

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
            var lastRaidUi = GetFromDbQueryHandler.GetLastRaidUi(parameters.LastClanTagMessage);

            var answer = CurrentStatisticsFunctions.GetCurrentRaidShortInfo(lastRaidUi);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
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