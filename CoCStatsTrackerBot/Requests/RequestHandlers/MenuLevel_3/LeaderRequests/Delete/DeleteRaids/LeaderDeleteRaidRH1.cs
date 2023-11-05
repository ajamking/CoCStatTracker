using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteRaidRH1 : BaseRequestHandler
{
    public LeaderDeleteRaidRH1()
    {
        Header = "Оставить последний.";
        HandlerMenuLevel = MenuLevel.LeaderDeleteRaidsMenu3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 1;

        var handler = new LeaderDeleteRaidsRHBase();

        handler.Execute(parameters);
    }
}