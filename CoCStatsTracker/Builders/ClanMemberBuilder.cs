using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace CoCStatsTracker.Builders;

public class ClanMemberBuilder
{
    public ClanMember ClanMember { get; } = new ClanMember();

    public ClanMemberBuilder(ClanMember clanMember = null)
    {
        if (clanMember != null)
        {
            ClanMember = clanMember;
        }

    }

    public void SetBaseProperties(PlayerApi playerApi)
    {
        ClanMember.UpdatedOn = DateTime.Now;
        ClanMember.Tag = playerApi.Tag;
        ClanMember.Name = playerApi.Name;

        ClanMember.TownHallLevel = playerApi.TownHallLevel;
        ClanMember.TownHallWeaponLevel = playerApi.TownHallWeaponLevel;
        ClanMember.ExpLevel = playerApi.ExpLevel;
        ClanMember.Trophies = playerApi.Trophies;
        ClanMember.BestTrophies = playerApi.BestTrophies;
        ClanMember.WarStars = playerApi.WarStars;
        ClanMember.AttackWins = playerApi.AttackWins;
        ClanMember.DefenceWins = playerApi.DefenseWins;

        ClanMember.BuilderHallLevel = playerApi.BuilderHallLevel;
        ClanMember.VersusTrophies = playerApi.VersusTrophies;
        ClanMember.BestVersusTrophies = playerApi.BestVersusTrophies;
        ClanMember.VersusBattleWins = playerApi.VersusBattleWins;

        ClanMember.Role = playerApi.RoleInClan;
        ClanMember.WarPreference = playerApi.WarPreference;

        ClanMember.DonationsSent = playerApi.DonationsSent;
        ClanMember.DonationsRecieved = playerApi.DonationsReceived;
        ClanMember.TotalCapitalContributions = playerApi.ClanCapitalContributions;

        ClanMember.League = playerApi.League.Name;
    }


    public void SetTrackedClan()
    {

    }

    public void SetCarma()
    {

    }

    public void SetUnits(TroopApi[] troops)
    {
        var units = new List<Unit>();

        foreach (var troop in troops)
        {
            var unit = new Unit();

            unit.Name = troop.Name;
            unit.Level = troop.Level;
            unit.Village = troop.Village;
            unit.SuperTroopIsActivated = troop.SuperTroopIsActivated;
            unit.Type = DefineUnitType(troop.Name);

            units.Add(unit);
        }
    }

    public void SetWarMembership()
    {

    }

    public void SetRaidMembership()
    {

    }

    public void SetDrawMembership()
    {

    }

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
