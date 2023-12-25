using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteRaidRH0 : BaseRequestHandler
{
    public LeaderDeleteRaidRH0()
    {
        Header = "Удалить все.";
        HandlerMenuLevel = MenuLevel.LeaderDeleteRaidsMenu3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 0;

        var handler = new LeaderDeleteRaidsRHBase();

        handler.Execute(parameters);
    }
}