using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System.Collections.Generic;

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