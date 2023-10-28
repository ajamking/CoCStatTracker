using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanRaidsHistoryRH3 : BaseRequestHandler
{
    public ClanRaidsHistoryRH3()
    {
        Header = "Последние 3";
        HandlerMenuLevel = MenuLevels.ClanRaidsHistory3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.EntriesCount = 3;

        var handler = new ClanRaidHistoryRHBase();

        handler.Execute(parameters);
    }
}
