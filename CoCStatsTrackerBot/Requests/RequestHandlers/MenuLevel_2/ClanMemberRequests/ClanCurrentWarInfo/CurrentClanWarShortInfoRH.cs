using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;

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
            var lastClanWarUi = GetFromDbQueryHandler.GetLastClanWarUi(parameters.LastClanTagMessage);

            var answer = CurrentStatisticsFunctions.GetCurrentWarShortInfo(lastClanWarUi);

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