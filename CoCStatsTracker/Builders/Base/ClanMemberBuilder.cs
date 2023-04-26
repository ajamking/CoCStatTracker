﻿using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Helpers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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

        if (playerApi.League != null)
        {
            ClanMember.League = playerApi.League.Name;
        }
        else
        {
            ClanMember.League = "Not Defined Yet";
        }
    }

    public void SetUnits(TroopApi[] troops, TroopApi[] heroes)
    {
        var units = new List<Troop>();

        foreach (var troop in troops)
        {
            var unit = new Troop();

            unit.Name = troop.Name;
            if (TroopDefiner.BaseUnitsForSupers.ContainsKey(unit.Name))
            {
                unit.Level = troops.FirstOrDefault(x => x.Name == TroopDefiner.BaseUnitsForSupers[unit.Name]).Level;
            }
            else
            {
                unit.Level = troop.Level;
            }

            unit.Village = troop.Village;
            unit.SuperTroopIsActivated = troop.SuperTroopIsActivated;
            unit.Type = TroopDefiner.DefineUnitType(troop.Name);

            units.Add(unit);
        }

        foreach (var troop in heroes)
        {
            var hero = new Troop();

            hero.Name = troop.Name;
            hero.Level = troop.Level;
            hero.Village = troop.Village;
            hero.SuperTroopIsActivated = troop.SuperTroopIsActivated;
            hero.Type = TroopDefiner.DefineUnitType(troop.Name);

            units.Add(hero);
        }

        ClanMember.Units = units;
    }

    public void SetCarma(Carma carma)
    {
        ClanMember.Carma = carma;
    }
    public void AddRaidMembership(RaidMember membership)
    {
        ClanMember.RaidMembership.Add(membership);
    }

    public void AddDrawMembership(DrawMember membership)
    {
        ClanMember.DrawMembership.Add(membership);
    }
}
