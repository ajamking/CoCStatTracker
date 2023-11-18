using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class CurrentClanWarMapRH : BaseRequestHandler
{
    public CurrentClanWarMapRH()
    {
        Header = "Карта";
        HandlerMenuLevel = MenuLevel.CurrentWarInfo2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var lastClanWarUi = GetFromDbQueryHandler.GetLastClanWarUi(parameters.LastClanTagMessage);

            var answer = CurrentStatisticsFunctions.GetCurrentWarMap(lastClanWarUi.WarMap);

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