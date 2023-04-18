using CoCStatsTracker.ApiEntities;
using Domain.Entities;

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
}
