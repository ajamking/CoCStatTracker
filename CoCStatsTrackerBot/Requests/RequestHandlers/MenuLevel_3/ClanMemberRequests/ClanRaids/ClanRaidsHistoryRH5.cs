using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class ClanRaidsHistoryRH5 : BaseRequestHandler
{
    public ClanRaidsHistoryRH5()
    {
        Header = "Последние_5";
        HandlerMenuLevel = MenuLevel.ClanRaidsHistory3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 5;

        var handler = new ClanRaidHistoryRHBase();

        handler.Execute(parameters);
    }
}