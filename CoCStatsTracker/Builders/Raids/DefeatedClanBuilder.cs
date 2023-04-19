using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System.Collections.Generic;

namespace CoCStatsTracker.Builders;

public class DefeatedClanBuilder
{
    public DefeatedClan Clan { get; } = new DefeatedClan();

    public DefeatedClanBuilder(DefeatedClan clan = null)
    {
        if (clan != null)
        {
            Clan = clan;
        }
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

    public void SetOpponentDistricts(ICollection<OpponentDistrict> districts)
    {
        Clan.DefeatedDistricts = districts;
    }
}
