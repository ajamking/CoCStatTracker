using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Items.Helpers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoCStatsTracker.Builders;

public class AttackedClanOnRaidBuilder
{
    public AttackedClanOnRaid AttackedClan { get; }

    public AttackedClanOnRaidBuilder(AttackedClanOnRaid attackedClan = null)
    {
        AttackedClan = attackedClan ?? new AttackedClanOnRaid();
    }

    public void SetBaseProperties(AttackedCapitalApi attackedCapital)
    {
        AttackedClan.Tag = attackedCapital.DefenderClan.Tag;
        AttackedClan.Name = attackedCapital.DefenderClan.Name;
        AttackedClan.Level = attackedCapital.DefenderClan.Level;
    }

    public void SetDestroyedDistricts(ICollection<DefeatedEmemyDistrict> destoyedDistricts)
    {
        AttackedClan.DefeatedEmemyDistricts = destoyedDistricts;
    }
}