using CoCStatsTracker.UIEntities;

namespace CoCStatsTrackerBot.Requests;
public static class FunctionsLogicHelper
{
    private static readonly Dictionary<int, int> _districtHallCostsByLvl = new()
    {
            {1,  135  }, {2,  225  },  {3,  350  }, {4,  405  }, {5,  460  },
    };

    private static readonly Dictionary<int, int> _capitalHallCostsByLvl = new()
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

    public static Dictionary<DistrictType, string> AllDistrictsRU { get; set; } = new Dictionary<DistrictType, string>()
    {
            {DistrictType.Capital_Peak, "Столичный пик" },
            {DistrictType.Barbarian_Camp, "Лагерь варваров" },
            {DistrictType.Wizard_Valley, "Долина колдунов" },
            {DistrictType.Balloon_Lagoon, "Лагуна шаров" },
            {DistrictType.Builders_Workshop, "Мастерская строителя" },
            {DistrictType.Dragon_Cliffs, "Драконьи утесы" },
            {DistrictType.Golem_Quarry, "Карьер големов" },
            {DistrictType.Skeleton_Park, "Парк скелетов" },
            {DistrictType.Goblin_Mines, "Гоблинские шахты" },
    };

    public static Dictionary<string, string> SuperUnitsMapper { get; set; } = new Dictionary<string, string>()
    {
                { "Super Barbarian", "Суперварвар"},
                { "Super Archer", "Суперлучница"},
                { "Super Giant", "Супергигант"},
                { "Sneaky Goblin", "Коварный гоблин"},
                { "Super Wall Breaker", "Суперстенобой"},
                { "Rocket Balloon", "Ракетный шар"},
                { "Super Wizard", "Суперколдун"},
                { "Super Dragon", "Супердракон"},
                { "Inferno Dragon", "Пламенный дракон"},
                { "Super Minion", "Суперминьон"},
                { "Super Valkyrie", "Супервалькирия"},
                { "Super Witch", "Суперведьма"},
                { "Ice Hound", "Ледяная гончая"},
                { "Super Bowler", "Супервышибала"},
                { "Super Miner", "Супершахтер"},
                { "Super Hog Rider", "Супервсадник на кабане"},
    };

    public static Dictionary<string, string> SiegeMachinesMapper { get; set; } = new Dictionary<string, string>()
    {
                { "Wall Wrecker", "Разрушитель стен"},
                { "Battle Blimp", "Боевой дирижабль"},
                { "Stone Slammer", "Камнебросатель"},
                { "Siege Barracks", "Осадные казармы"},
                { "Log Launcher", "Бревномет"},
                { "Flame Flinger", "Огнеметатель"},
                { "Battle Drill", "Боевой бур"},
    };


    public static List<DistrictUi> SortDistrictsAsOnMap(this ICollection<DistrictUi> districts)
    {
        var newDistrictsList = new List<DistrictUi>();

        foreach (var districtType in AllDistricts)
        {
            newDistrictsList.AddRange(districts.Where(x => x.Name == districtType.Value));
        }

        return newDistrictsList;
    }

    public static RaidPrediction GetCurrentRaidMedalsRewardPrediction(CapitalRaidUi raidsUi)
    {
        var predict = new RaidPrediction
        {
            OffensePrediction = GetOffensePrediction(raidsUi),

            DefensePrediction = GetDefensePrediction(raidsUi),
        };

        predict.SummPrediction = predict.DefensePrediction + predict.OffensePrediction;

        return predict;
    }

    public static Dictionary<string, string> GetAllMachineLevels(ArmyUi memberArmy, List<string> allMachinesInGame)
    {
        var machineLevels = new Dictionary<string, string>();

        foreach (var machine in allMachinesInGame)
        {
            if (memberArmy.SiegeMachines.Any(x => x.Name == machine))
            {
                machineLevels.Add(machine, memberArmy.SiegeMachines.First(x => x.Name == machine).Lvl.ToString());
            }
            else
            {
                machineLevels.Add(machine, "0");
            }
        }

        return machineLevels;
    }

    public static string GetTimeLeft(this DateTime endenOn)
    {
        return $"{Math.Round(endenOn.Subtract(DateTime.Now).TotalHours, 0)}ч. {endenOn.Subtract(DateTime.Now).Minutes}м.";
    }


    private static int GetOffensePrediction(CapitalRaidUi raidsUi)
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
            catch (Exception)
            {

            }

        }

        /*raidsUi.TotalAttacksCount*/
        var capitalHallsCount = destroyedDistricts.DefeatedCapitalHalls.Sum(chPair => chPair.Value * _capitalHallCostsByLvl[chPair.Key]);

        var otherDistrictsCount = destroyedDistricts.DefeatedOtherDistricts.Sum(dhPair => dhPair.Value * _districtHallCostsByLvl[dhPair.Key]);

        var offenseResult = (capitalHallsCount + otherDistrictsCount) / maxTotalRaidsAttakcs * maxMemberAttacksPerRaid;

        return offenseResult + magicNumber;
    }

    private static int GetDefensePrediction(CapitalRaidUi raidsUi)
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
            if (district.TotalDestructionPercent <= 100 && district.AttacksCount == 5)
            {
                summDeadUnits += maxDedUnitsPerAttack * 4 + 100;
            }
            if (district.TotalDestructionPercent <= 100 && district.AttacksCount >= 6)
            {
                summDeadUnits += maxDedUnitsPerAttack * 5 + 100;
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
    private static readonly int _chCapacity = 10;

    private static readonly int _dhCapacity = 5;

    public Dictionary<int, int> DefeatedCapitalHalls = new(_chCapacity);

    public Dictionary<int, int> DefeatedOtherDistricts = new(_dhCapacity);

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