using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteRaidRH3 : BaseRequestHandler
{
    public LeaderDeleteRaidRH3()
    {
        Header = "Оставить 3 последних.";
        HandlerMenuLevel = MenuLevel.LeaderDeleteRaidsMenu3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 3;

        var handler = new LeaderDeleteRaidsRHBase();

        handler.Execute(parameters);
    }
}