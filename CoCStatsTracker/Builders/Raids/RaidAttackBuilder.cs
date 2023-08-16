using CoCStatsTracker.ApiEntities;
using Domain.Entities;

namespace CoCStatsTracker.Builders;

public class RaidAttackBuilder
{
    public RaidAttack RaidAttack { get; }

    public RaidAttackBuilder(RaidAttack raidAttack = null)
    {
        RaidAttack = raidAttack ?? new RaidAttack();
    }

    public void SetBaseProperties(int destructionPercentFrom, AttackOnDistrictApi attack, OpponentDistrict opponentDistrict)
    {
        RaidAttack.DestructionPercentFrom = destructionPercentFrom;
        RaidAttack.DestructionPercentTo = attack.DestructionPercentTo;
        RaidAttack.MemberTag = attack.Attacker.Tag;
        RaidAttack.MemberName = attack.Attacker.Name;
        RaidAttack.OpponentDistrict = opponentDistrict;
    }

    public void SetRaidMember(RaidMember raidMember)
    {
        RaidAttack.RaidMember = raidMember;
    }
}
