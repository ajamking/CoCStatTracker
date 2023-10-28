using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanWarHistoryRHBase : BaseRequestHandler
{
    public ClanWarHistoryRHBase()
    {
        Header = "Вызывается только через другие методы";
        HandlerMenuLevel = MenuLevels.ClanWarsHistory3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var clanWars = GetFromDbQueryHandler.GetAllClanWars(parameters.LastTagMessage);

            var answer = ClanFunctions.GetClanWarHistory(clanWars, parameters.EntriesCount, MessageSplitToken);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
