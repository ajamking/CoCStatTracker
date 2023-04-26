using CoCStatsTracker.ApiEntities;
using Domain.Entities;

namespace CoCStatsTracker.Builders;

public class RaidAttackBuilder
{
    public RaidAttack RaidAttack { get; } = new RaidAttack();

    public RaidAttackBuilder(RaidAttack raidAttack = null)
    {
        if (raidAttack != null)
        {
            RaidAttack = raidAttack;
        }
    }

    public void SetBaseProperties(AttackOnDistrictApi attack, int destructionPercentFrom)
    {
        RaidAttack.DestructionPercentTo = attack.DestructionPercentTo;
        RaidAttack.MemberTag = attack.Attacker.Tag;
        RaidAttack.MemberName = attack.Attacker.Name;
        RaidAttack.DestructionPercentFrom = destructionPercentFrom;
    }

    public void SetRaidMember(RaidMember raidMember)
    {
        RaidAttack.RaidMember = raidMember;
    }
}
