using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteWarsRH1 : BaseRequestHandler
{
    public LeaderDeleteWarsRH1()
    {
        Header = "Оставить последнюю";
        HandlerMenuLevel = MenuLevel.LeaderDeleteClanWarsMenu3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 1;

        var handler = new LeaderDeleteWarsRHBase();

        handler.Execute(parameters);
    }
}