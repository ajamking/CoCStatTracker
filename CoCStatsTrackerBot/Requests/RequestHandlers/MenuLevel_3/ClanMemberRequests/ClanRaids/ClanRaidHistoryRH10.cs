using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanRaidHistoryRH10 : BaseRequestHandler
{
    public ClanRaidHistoryRH10()
    {
        Header = "Последние_10";
        HandlerMenuLevel = MenuLevel.ClanWarsHistory3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 10;

        var handler = new ClanRaidHistoryRHBase();

        handler.Execute(parameters);
    }
}