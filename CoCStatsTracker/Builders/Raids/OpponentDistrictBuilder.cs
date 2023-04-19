using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System.Collections.Generic;

namespace CoCStatsTracker.Builders;
public class OpponentDistrictBuilder
{
    public OpponentDistrict District { get; } = new OpponentDistrict();

    public OpponentDistrictBuilder(OpponentDistrict district = null)
    {
        if (district != null)
        {
            District = district;
        }
    }

    public void SetBaseProperties(DistrictApi district)
    {
        District.Name = district.Name;
        District.Level = district.DistrictLevel;

    }

    public void SetAttacks(ICollection<RaidAttack> attacks)
    {

        District.Attacks = attacks;
    }
}
