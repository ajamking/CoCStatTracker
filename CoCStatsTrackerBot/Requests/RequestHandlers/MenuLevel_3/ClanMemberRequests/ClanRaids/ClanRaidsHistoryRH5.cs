using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanRaidsHistoryRH5 : BaseRequestHandler
{
    public ClanRaidsHistoryRH5()
    {
        Header = "Последние 5";
        HandlerMenuLevel = MenuLevels.ClanRaidsHistory3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.EntriesCount = 5;

        var handler = new ClanRaidHistoryRHBase();

        handler.Execute(parameters);
    }
}
