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

    public void SetBaseProperties(AttackOnDistrictApi memberAttackApi, AttackedCapitalApi raidOnClanApi,
        DistrictApi destroyedDistrictApi, int previousDestructionPercent)
    {
        RaidAttack = new RaidAttack()
        {
            MemberTag = memberAttackApi.Attacker.Tag,
            MemberName = memberAttackApi.Attacker.Name,

            OpponentClanTag = raidOnClanApi.DefenderClan.Tag,
            OpponentClanName = raidOnClanApi.DefenderClan.Name,
            OpponentClanLevel = raidOnClanApi.DefenderClan.Level,
            OpponentDistrictName = destroyedDistrictApi.Name,
            OpponentDistrictLevel = destroyedDistrictApi.DistrictLevel,

            DestructionPercentTo = memberAttackApi.DestructionPercentTo,
            DestructionPercentFrom = previousDestructionPercent
        };
    }

    public void SetRaidMember(RaidMember raidMember)
    {
        RaidAttack.RaidMember = raidMember;
    }
}
