using CoCStatsTracker.UIEntities;

namespace CoCStatsTrackerBot.Requests;
public static class FunctionsLogicHelper
{
    private static Dictionary<int, int> _districtHallCostsByLvl = new Dictionary<int, int>()
    {
            {1,  135  }, {2,  225  },  {3,  350  }, {4,  405  }, {5,  460  },
    };

    private static Dictionary<int, int> _capitalHallCostsByLvl = new Dictionary<int, int>()
    {
            {2,  180  }, {3,  360  }, {4,  585  }, {5,  810  }, {6,  1115 }, {7,  1240 },  {8,  1260 }, {9,  1375 }, {10, 1450 },
    };

    public static Dictionary<DistrictType, string> AllDistricts { get; set; } = new Dictionary<DistrictType, string>()
    {
            {DistrictType.Capital_Peak, "Capital Peak" },
            {DistrictType.Barbarian_Camp, "Barbarian Camp" },
            {DistrictType.Wizard_Valley, "Wizard Valley" },
            {DistrictType.Balloon_Lagoon, "Balloon Lagoon" },
            {DistrictType.Builders_Workshop, "Builder's Workshop" },
            {DistrictType.Dragon_Cliffs, "Dragon Cliffs" },
            {DistrictType.Golem_Quarry, "Golem Quarry" },
            {DistrictType.Skeleton_Park, "Skeleton Park" },
            {DistrictType.Goblin_Mines, "Goblin Mines" },
    };

    public static List<DistrictUi> SortAsOnMap(this ICollection<DistrictUi> districts)
    {
        var newDistrictsList = new List<DistrictUi>();

        foreach (var districtType in AllDistricts)
        {
            newDistrictsList.AddRange(districts.Where(x => x.Name == districtType.Value));
        }

        return newDistrictsList;
    }

    public static RaidPrediction GetCurrentRaidMedalsRewardPrediction(RaidUi raidsUi)
    {
        var predict = new RaidPrediction();

        predict.OffensePrediction = GetOffensePrediction(raidsUi);

        predict.DefensePrediction = GetDefensePrediction(raidsUi);

        predict.SummPrediction = predict.DefensePrediction + predict.OffensePrediction;

        return predict;
    }

    private static int GetOffensePrediction(RaidUi raidsUi)
    {
        var maxMemberAttacksPerRaid = 6;

        var maxTotalRaidsAttakcs = 300;

        var magicNumber = 6;

        var destroyedDistricts = new DistrictsForPrediction();

        foreach (var district in raidsUi.DefeatedClans.SelectMany(x => x.DefeatedEmemyDistricts).ToList())
        {
            try
            {
                if (district.TotalDestructionPercent == 0)
                {
                    continue;
                }
                if (district.Name == AllDistricts[DistrictType.Capital_Peak])
                {
                    destroyedDistricts.DefeatedCapitalHalls[district.Level]++;
                }
                else
                {
                    destroyedDistricts.DefeatedOtherDistricts[district.Level]++;
                }
            }
            catch (Exception e)
            {

            }

        }

        /*raidsUi.TotalAttacksCount*/
        var capitalHallsCount = destroyedDistricts.DefeatedCapitalHalls.Sum(chPair => chPair.Value * _capitalHallCostsByLvl[chPair.Key]);

        var otherDistrictsCount = destroyedDistricts.DefeatedOtherDistricts.Sum(dhPair => dhPair.Value * _districtHallCostsByLvl[dhPair.Key]);

        var offenseResult = (capitalHallsCount + otherDistrictsCount) / maxTotalRaidsAttakcs * maxMemberAttacksPerRaid;

        return offenseResult + magicNumber;
    }

    private static int GetDefensePrediction(RaidUi raidsUi)
    {
        var betterDefense = raidsUi.Defenses.OrderByDescending(defense => defense.TotalAttacksCount).FirstOrDefault();

        if (betterDefense == null)
        {
            return 0;
        }

        var summDeadUnits = 0;

        var magicDivider = 25;

        var maxDedUnitsPerAttack = 250;

        foreach (var district in betterDefense.DestroyedFriendlyDistricts)
        {
            if (district.TotalDestructionPercent <= 100 && district.AttacksCount == 0)
            {
                summDeadUnits += 0;
            }
            if (district.TotalDestructionPercent <= 100 && district.AttacksCount == 1)
            {
                summDeadUnits += maxDedUnitsPerAttack - 50;
            }
            if (district.TotalDestructionPercent <= 100 && district.AttacksCount == 2)
            {
                summDeadUnits += maxDedUnitsPerAttack + 200;
            }
            if (district.TotalDestructionPercent <= 100 && district.AttacksCount == 3)
            {
                summDeadUnits += maxDedUnitsPerAttack * 2 + 150;
            }
            if (district.TotalDestructionPercent <= 100 && district.AttacksCount == 4)
            {
                summDeadUnits += maxDedUnitsPerAttack * 3 + 50;
            }
            if (district.TotalDestructionPercent <= 100 && district.AttacksCount >= 5)
            {
                summDeadUnits += maxDedUnitsPerAttack * 4 + 50;
            }
            else
            {
                summDeadUnits += 0;
            }
        }

        return summDeadUnits / magicDivider;
    }
}

public class DistrictsForPrediction
{
    private static int _chCapacity = 10;

    private static int _dhCapacity = 5;

    public Dictionary<int, int> DefeatedCapitalHalls = new Dictionary<int, int>(_chCapacity);

    public Dictionary<int, int> DefeatedOtherDistricts = new Dictionary<int, int>(_dhCapacity);

    public DistrictsForPrediction()
    {
        for (int i = 2; i <= _chCapacity; i++)
        {
            DefeatedCapitalHalls[i] = 0;
        }

        for (int i = 1; i <= _dhCapacity; i++)
        {
            DefeatedOtherDistricts[i] = 0;
        }
    }
}

public class RaidPrediction
{
    public int OffensePrediction { get; set; }

    public int DefensePrediction { get; set; }

    public int SummPrediction { get; set; }
}

public enum DistrictType
{
    Capital_Peak = 1,
    Barbarian_Camp,
    Wizard_Valley,
    Balloon_Lagoon,
    Builders_Workshop,
    Dragon_Cliffs,
    Golem_Quarry,
    Skeleton_Park,
    Goblin_Mines
}