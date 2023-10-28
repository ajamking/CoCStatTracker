using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanRaidHistoryRHBase : BaseRequestHandler
{
    public ClanRaidHistoryRHBase()
    {
        Header = "Вызывается только через другие методы";
        HandlerMenuLevel = MenuLevels.ClanRaidsHistory3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var raids = GetFromDbQueryHandler.GetAllRaids(parameters.LastTagMessage);

            var answer = ClanFunctions.GetRaidsHistory(raids, parameters.EntriesCount, MessageSplitToken);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
