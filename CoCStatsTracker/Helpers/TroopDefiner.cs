using Domain.Entities;

namespace CoCStatsTracker.Helpers;
public static class TroopDefiner
{
    public static UnitType DefineUnitType(string name)
    {
        if (name == "Barbarian King" || name == "Archer Queen" || name == "Grand Warden"
            || name == "Royal Champion" || name == "Battle Machine")
        {
            return UnitType.Hero;
        }

        else if (name == "Wall Wrecker" || name == "Battle Blimp" || name == "Stone Slammer"
            || name == "Siege Barracks" || name == "Log Launcher" || name == "Flame Flinger"
            || name == "Battle Drill")
        {
            return UnitType.SiegeMachine;
        }

        else if (name == "Super Barbarian" || name == "Super Archer" || name == "Super Wall Breaker"
            || name == "Super Giant" || name == "Rocket Balloon" || name == "Sneaky Goblin"
            || name == "Super Miner" || name == "Inferno Dragon" || name == "Super Valkyrie"
            || name == "Super Witch" || name == "Ice Hound" || name == "Super Bowler"
            || name == "Super Dragon" || name == "Super Wizard" || name == "Super Minion")
        {
            return UnitType.SuperUnit;
        }

        else if (name == "L.A.S.S.I" || name == "Mighty Yak" || name == "Electro Owl"
          || name == "Unicorn" || name == "Phoenix" || name == "Poison Lizard"
          || name == "Diggy" || name == "Frosty")
        {
            return UnitType.Pet;
        }

        else return UnitType.Unit;
    }
}
