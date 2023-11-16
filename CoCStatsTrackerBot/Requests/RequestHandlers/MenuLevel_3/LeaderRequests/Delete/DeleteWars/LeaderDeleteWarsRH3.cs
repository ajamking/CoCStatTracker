using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteWarsRH3 : BaseRequestHandler
{
    public LeaderDeleteWarsRH3()
    {
        Header = "Оставить 3 последних";
        HandlerMenuLevel = MenuLevel.LeaderDeleteClanWarsMenu3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 3;

        var handler = new LeaderDeleteWarsRHBase();

        handler.Execute(parameters);
    }
}