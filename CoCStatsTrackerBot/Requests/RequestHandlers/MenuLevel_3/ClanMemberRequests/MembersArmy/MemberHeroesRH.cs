using CoCStatsTrackerBot.Menu;
using Domain.Entities;

namespace CoCStatsTrackerBot.Requests;

public class MemberHeroesRH : BaseRequestHandler
{
    public MemberHeroesRH()
    {
        Header = "Герои";
        HandlerMenuLevel = MenuLevels.PlayerArmy3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        parameters.UnitType = UnitType.Hero;

        var handler = new MemberArmyRHBase();

        handler.Execute(parameters);
    }
}
