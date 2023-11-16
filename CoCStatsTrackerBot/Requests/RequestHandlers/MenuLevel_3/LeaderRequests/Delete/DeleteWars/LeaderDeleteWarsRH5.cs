using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteWarsRH5 : BaseRequestHandler
{
    public LeaderDeleteWarsRH5()
    {
        Header = "Оставить 5 последних";
        HandlerMenuLevel = MenuLevel.LeaderDeleteClanWarsMenu3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.EntriesCount = 5;

        var handler = new LeaderDeleteWarsRHBase();

        handler.Execute(parameters);
    }
}