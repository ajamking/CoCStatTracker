using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteWarsRH0 : BaseRequestHandler
{
    public LeaderDeleteWarsRH0()
    {
        Header = "Удалить все";
        HandlerMenuLevel = MenuLevel.LeaderDeleteClanWarsMenu3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 0;

        var handler = new LeaderDeleteWarsRHBase();

        handler.Execute(parameters);
    }
}