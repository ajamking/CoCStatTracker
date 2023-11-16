using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CoCStatsTracker.Builders;

public class RaidDefenseBuilder
{
    public ICollection<RaidDefense> Defenses { get; set; }

    public RaidDefenseBuilder(ICollection<RaidDefense> defenses = null)
    {
        Defenses = defenses ?? new List<RaidDefense>();
    }

    public void SetBaseProperties(DefenseApi[] defenses)
    {
        var raidDefenses = new List<RaidDefense>();

        foreach (var defense in defenses)
        {
            var raidDefense = new RaidDefense
            {
                AttackerClanTag = defense.AttackerClan.Tag,
                AttackerClanName = defense.AttackerClan.Name,
                AttackerClanLevel = defense.AttackerClan.Level,
                TotalAttacksCount = defense.AttackCount,
                DestroyedFriendlyDistrictsCount = defense.DistrictsDestroyedCount,
                DestroyedFriendlyDistricts = SetDestroyedFriendlyDistricts(defense.DistrictsDestroyed),

                TotalEnemyLoot = defense.DistrictsDestroyed.Sum(district => district.TotalLooted)
            };

            raidDefenses.Add(raidDefense);
        }

        Defenses = raidDefenses;
    }

    public void SetRaid(CapitalRaid raid)
    {
        foreach (var defence in Defenses)
        {
            defence.CapitalRaid = raid;
        }
    }

    private static List<DestroyedFriendlyDistrict> SetDestroyedFriendlyDistricts(DistrictApi[] destrpyedDistricts)
    {
        var districts = new List<DestroyedFriendlyDistrict>();

        foreach (var district in destrpyedDistricts)
        {
            var tempDistrict = new DestroyedFriendlyDistrict
            {
                Name = district.Name,
                Level = district.DistrictLevel,
                AttacksSpent = district.AttackCount,
                TotalDestructionPersent = district.DestructionPercent
            };

            districts.Add(tempDistrict);
        }

        return districts;
    }
}
