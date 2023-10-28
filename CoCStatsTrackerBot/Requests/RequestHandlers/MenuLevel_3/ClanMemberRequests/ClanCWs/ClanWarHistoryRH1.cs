using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanWarHistoryRH1 : BaseRequestHandler
{
    public ClanWarHistoryRH1()
    {
        Header = "Последняя война";
        HandlerMenuLevel = MenuLevels.ClanWarsHistory3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.EntriesCount = 1;

        var handler = new ClanWarHistoryRHBase();

        handler.Execute(parameters);
    }
}
