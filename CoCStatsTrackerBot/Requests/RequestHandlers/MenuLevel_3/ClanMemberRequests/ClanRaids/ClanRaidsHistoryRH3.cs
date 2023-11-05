using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanRaidsHistoryRH3 : BaseRequestHandler
{
    public ClanRaidsHistoryRH3()
    {
        Header = "Последние_3";
        HandlerMenuLevel = MenuLevel.ClanRaidsHistory3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 3;

        var handler = new ClanRaidHistoryRHBase();

        handler.Execute(parameters);
    }
}
