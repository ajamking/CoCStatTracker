﻿using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System.Collections.Generic;

namespace CoCStatsTracker.Builders;

public class RaidDefenseBuilder
{
    public ICollection<RaidDefense> Defenses { get; set; } = new List<RaidDefense>();

    public RaidDefenseBuilder(ICollection<RaidDefense> defenses = null)
    {
        if (defenses != null)
        {
            Defenses = defenses;
        }
    }

    public void SetBaseProperties(DefenseApi[] defenses)
    {
        foreach (var defense in defenses)
        {
            var raidDefense = new RaidDefense();

            raidDefense.AttackerClanTag = defense.AttackerClan.Tag;
            raidDefense.AttackerClanName = defense.AttackerClan.Name;
            raidDefense.AttackerClanLevel = defense.AttackerClan.Level;
            raidDefense.TotalAttacksCount = defense.AttackCount;
            raidDefense.DistrictsDestroyed = defense.DistrictsDestroyedCount;

            raidDefense.DestroyedFriendlyDistricts = SetDestroyedFriendlyDistricts(defense.DistrictsDestroyed);

            Defenses.Add(raidDefense);
        }
    }

    private List<DestroyedFriendlyDistrict> SetDestroyedFriendlyDistricts(DistrictApi[] destrpyedDistricts)
    {
        var districts = new List<DestroyedFriendlyDistrict>();

        foreach (var district in destrpyedDistricts)
        {
            var tempDistrict = new DestroyedFriendlyDistrict();

            tempDistrict.Name = district.Name;
            tempDistrict.Level = district.DistrictLevel;
            tempDistrict.AttacksSpent = district.AttackCount;
            tempDistrict.TotalDestructionPersent = district.DestructionPercent;

            districts.Add(tempDistrict);
        }

        return districts;
    }
}