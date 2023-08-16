using CoCStatsTracker.ApiEntities;
using Domain.Entities;
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
        Member.TotalLoot = member.CapitalResourcesLooted;
        Member.Tag = member.Tag;
        Member.Name = member.Name;
    }

    public void SetRaidMemberAttacks(ICollection<RaidAttack> attacks)
    {
        Member.Attacks = attacks;
    }

    public void SetRaid(CapitalRaid raid)
    {
        Member.Raid = raid;
    }

    public void SetClanMember(ClanMember clanMember)
    {
        Member.ClanMember = clanMember;
    }
}
