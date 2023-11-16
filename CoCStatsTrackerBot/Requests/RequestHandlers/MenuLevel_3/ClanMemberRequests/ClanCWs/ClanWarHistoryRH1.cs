using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class ClanWarHistoryRH1 : BaseRequestHandler
{
    public ClanWarHistoryRH1()
    {
        Header = "Последняя война";
        HandlerMenuLevel = MenuLevel.ClanWarsHistory3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 1;

        var handler = new ClanWarHistoryRHBase();

        handler.Execute(parameters);
    }
}