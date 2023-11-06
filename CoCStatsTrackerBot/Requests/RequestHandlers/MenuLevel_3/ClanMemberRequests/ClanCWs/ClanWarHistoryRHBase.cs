using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanWarHistoryRHBase : BaseRequestHandler
{
    public ClanWarHistoryRHBase()
    {
        Header = "Вызывается только через другие методы";
        HandlerMenuLevel = MenuLevel.ClanWarsHistory3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var clanWars = GetFromDbQueryHandler.GetAllClanWarsUi(parameters.LastClanTagMessage);

            var answer = ClanFunctions.GetClanWarHistory(clanWars, parameters.EntriesCount, MessageSplitToken);

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
