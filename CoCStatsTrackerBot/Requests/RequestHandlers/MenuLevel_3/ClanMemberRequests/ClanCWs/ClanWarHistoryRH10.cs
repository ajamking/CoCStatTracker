using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class ClanWarHistoryRH10 : BaseRequestHandler
{
    public ClanWarHistoryRH10()
    {
        Header = "Последние 10";
        HandlerMenuLevel = MenuLevel.ClanWarsHistory3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 10;

        var handler = new ClanWarHistoryRHBase();

        handler.Execute(parameters);
    }
}