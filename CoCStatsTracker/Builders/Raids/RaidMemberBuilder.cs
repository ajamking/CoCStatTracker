using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System.Collections.Generic;

namespace CoCStatsTracker.Builders;

public class RaidMemberBuilder
{
    public RaidMember Member { get; } = new RaidMember();

    public RaidMemberBuilder(RaidMember raidMember = null)
    {
        if (raidMember != null)
        {
            Member = raidMember;
        }
    }

    public void SetBaseProperties(RaidMemberApi member)
    {
        Member.TotalLoot = member.CapitalResourcesLooted;
        Member.Tag = member.Tag;
        Member.Name = member.Name;
    }

}
