using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Items.Helpers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoCStatsTracker.Builders;

public class CapitalRaidBuilder
{
    public CapitalRaid Raid { get; }

    public CapitalRaidBuilder(CapitalRaid raid = null)
    {
        Raid = raid ?? new CapitalRaid();
    }

    public void SetBaseProperties(RaidApi raidApi)
    {
        Raid.UpdatedOn = DateTime.Now;

        Raid.State = raidApi.State;
        Raid.StartedOn = DateTimeParser.ParseToDateTime(raidApi.StartTime).ToLocalTime();
        Raid.EndedOn = DateTimeParser.ParseToDateTime(raidApi.EndTime).ToLocalTime();
        Raid.TotalLoot = raidApi.CapitalTotalLoot;
        Raid.TotalAttacks = raidApi.TotalAttacks;
        Raid.EnemyDistrictsDestoyed = raidApi.EnemyDistrictsDestroyed;
        Raid.OffensiveReward = raidApi.OffensiveRewardBase * 6;
        Raid.DefenSiveReward = raidApi.DefensiveReward;
    }

    public void SetTrackedClan(TrackedClan clan)
    {
        Raid.TrackedClan = clan;
    }

    public void SetRaidDefenses(ICollection<RaidDefense> defenses)
    {
        Raid.RaidDefenses = defenses;
    }

    public void SetAttackedClans(ICollection<AttackedClanOnRaid> attackedClans)
    {
        Raid.AttackedClans = attackedClans;
    }

    public void SetRaidMembers(ICollection<RaidMember> members)
    {
        Raid.RaidMembers = members;
    }
}