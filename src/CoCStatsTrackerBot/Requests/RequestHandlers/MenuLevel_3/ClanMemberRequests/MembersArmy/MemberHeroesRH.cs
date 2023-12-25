using CoCStatsTrackerBot.BotMenues;
using Domain.Entities;

namespace CoCStatsTrackerBot.Requests;

public class MemberHeroesRH : BaseRequestHandler
{
    public MemberHeroesRH()
    {
        Header = "Герои";
        HandlerMenuLevel = MenuLevel.PlayerArmy3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        parameters.UnitType = UnitType.Hero;

        var handler = new MemberArmyRHBase();

        handler.Execute(parameters);
    }
}