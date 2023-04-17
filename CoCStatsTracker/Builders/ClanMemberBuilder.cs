using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Helpers;
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
        var units = new List<Troop>();

        foreach (var troop in troops)
        {
            var unit = new Troop();

            unit.Name = troop.Name;
            unit.Level = troop.Level;
            unit.Village = troop.Village;
            unit.SuperTroopIsActivated = troop.SuperTroopIsActivated;
            unit.Type = TroopDefiner.DefineUnitType(troop.Name);

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


}
