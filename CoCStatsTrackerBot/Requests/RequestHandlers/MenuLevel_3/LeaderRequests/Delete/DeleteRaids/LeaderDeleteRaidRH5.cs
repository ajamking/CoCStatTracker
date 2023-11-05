using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteRaidRH5 : BaseRequestHandler
{
    public LeaderDeleteRaidRH5()
    {
        Header = "Оставить 5 последних.";
        HandlerMenuLevel = MenuLevel.LeaderDeleteRaidsMenu3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 5;

        var handler = new LeaderDeleteRaidsRHBase();

        handler.Execute(parameters);
    }
}