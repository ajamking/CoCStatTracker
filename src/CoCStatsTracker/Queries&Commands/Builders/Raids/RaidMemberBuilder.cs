using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace CoCStatsTracker.Builders;

public class RaidMemberBuilder
{
    public RaidMember Member { get; }

    public RaidMemberBuilder(RaidMember raidMember = null)
    {
        Member = raidMember ?? new RaidMember();
    }

    public void SetBaseProperties(RaidMemberApi member)
    {
        Member.UpdatedOn = DateTime.Now;

        Member.TotalLoot = member.CapitalResourcesLooted;
        Member.Tag = member.Tag;
        Member.Name = member.Name;
    }

    public void SetRaid(CapitalRaid raid)
    {
        Member.CapitalRaid = raid;
    }

    public void SetRaidMemberAttacks(ICollection<RaidAttack> attacks)
    {
        Member.Attacks = attacks;
    }

    public void SetClanMember(ClanMember clanMember)
    {
        Member.ClanMember = clanMember;
    }
}