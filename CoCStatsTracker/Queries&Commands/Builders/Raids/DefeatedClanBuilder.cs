using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System.Collections.Generic;

namespace CoCStatsTracker.Builders;

public class DefeatedClanBuilder
{
    public DefeatedClan Clan { get; }

    public DefeatedClanBuilder(DefeatedClan clan = null)
    {
        Clan = clan ?? new DefeatedClan();
    }

    public void SetBaseProperties(AttackedCapitalApi clan)
    {
        Clan.DefendersTag = clan.DefenderClan.Tag;
        Clan.DefendersName = clan.DefenderClan.Name;
        Clan.DefendersLevel = clan.DefenderClan.Level;
        Clan.DistrictsCount = clan.DistrictCount;
        Clan.DistrictsDestroyedCount = clan.DistrictsDestroyedCount;
        Clan.AttacksSpentCount = clan.AttackCount;
    }

    public void SetCapitalRaid(CapitalRaid raid)
    {
        Clan.CapitalRaid = raid;
    }

    public void SetOpponentDistricts(ICollection<OpponentDistrict> districts)
    {
        Clan.DefeatedDistricts = districts;
    }
}
