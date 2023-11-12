using CoCStatsTracker.ApiEntities;
using Domain.Entities;

namespace CoCStatsTracker.Builders;

public class RaidAttackBuilder
{
    public RaidAttack RaidAttack { get; set; }

    public RaidAttackBuilder(RaidAttack raidAttack = null)
    {
        RaidAttack = raidAttack ?? new RaidAttack();
    }

    public void SetBaseProperties(int destructionPercentFrom, int destructionPercentTo)
    {
        RaidAttack.DestructionPercentFrom = destructionPercentFrom;
        RaidAttack.DestructionPercentTo = destructionPercentTo;
    }

    public void SetDefeatedDistrict(DefeatedEmemyDistrict defeatedEmemyDistrict)
    {
        RaidAttack.DefeatedEmemyDistrict = defeatedEmemyDistrict;
    }

    public void SetAttackedClan(AttackedClanOnRaid raidOnClan)
    {
        RaidAttack.AttackedClan = raidOnClan;
    }

    public void SetRaidMember(RaidMember raidMember)
    {
        RaidAttack.RaidMember = raidMember;
    }
}