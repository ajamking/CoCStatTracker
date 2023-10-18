using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Helpers;
using Domain.Entities;
using System;
using System.Collections.Generic;

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
        Raid.State = raidApi.State;
        Raid.StartedOn = DateTimeParser.Parse(raidApi.StartTime).ToLocalTime();
        Raid.EndedOn = DateTimeParser.Parse(raidApi.EndTime).ToLocalTime();
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

    public void SetRaidMembers(ICollection<RaidMember> members)
    {
        Raid.RaidMembers = members;
    }

    public void SetDefeatedClans(ICollection<DefeatedClan> clans)
    {
        Raid.DefeatedClans = clans;
    }

    public void SetAttacks(ICollection<RaidAttack> attacks)
    {
        Raid.RaidAttacks = attacks;
    }

    public void SetRaidDefenses(ICollection<RaidDefense> defenses)
    {
        Raid.RaidDefenses = defenses;
    }
}
