using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System.Collections.Generic;

namespace CoCStatsTracker.Builders;

public class RaidDefenseBuilder
{
    public RaidDefense Defense { get; set; } = new RaidDefense();

    public RaidDefenseBuilder(RaidDefense defense = null)
    {
        if (defense != null)
        {
            Defense = defense;
        }
    }

    public void SetBaseProperties(RaidDefense defens)
    {
        Defense.AttackerClanTag = defens.AttackerClanTag;
        Defense.AttackerClanName = defens.AttackerClanName;
        Defense.AttackerClanLevel = defens.AttackerClanLevel;
        Defense.TotalAttacksCount = defens.TotalAttacksCount;
        Defense.DistrictsDestroyed = defens.DistrictsDestroyed;
    }

    public void SetDestroyedFriendlyDistricts(ICollection<DistrictApi> destrpyedDistricts)
    {
        var tempDistrict = new DestroyedFriendlyDistrict();

        foreach (var district in destrpyedDistricts)
        {
            tempDistrict.Name = district.Name;
            tempDistrict.Level = district.DistrictLevel;
            tempDistrict.AttacksSpent = district.AttackCount;
            tempDistrict.TotalDestructionPersent = district.DestructionPercent;

            Defense.DestroyedFriendlyDistricts.Add(tempDistrict);
        }
    }
}
