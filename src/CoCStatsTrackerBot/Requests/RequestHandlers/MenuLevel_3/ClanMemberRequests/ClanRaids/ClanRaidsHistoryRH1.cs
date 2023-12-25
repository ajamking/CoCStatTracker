using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class ClanRaidsHistoryRH1 : BaseRequestHandler
{
    public ClanRaidsHistoryRH1()
    {
        Header = "Последний рейд";
        HandlerMenuLevel = MenuLevel.ClanRaidsHistory3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 1;

        var handler = new ClanRaidHistoryRHBase();

        handler.Execute(parameters);
    }
}