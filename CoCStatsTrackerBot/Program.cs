using CoCApiDealer.ApiRequests;
using CoCStatsTracker;
using CoCStatsTracker.ApiEntities;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot;

/// <summary>
/// Тег клана:	"#YPPGCCY8", "#UQQGYJJP", "#VUJCUQ9Y"
/// 
/// Тег игрока: AJAMKING: #G8P9Q299R Зануда051: #LRPLYJ9U2
/// </summary>

class Program
{
    public static double PredictPerformanceNew(double loot, double avgLoot, double avgDefLoot)
    {
        var threshold = 690;

        var center = Math.Pow(loot, 0.6) + 20;

        var skill = avgLoot - avgDefLoot;

        if (skill > threshold)
        {
            skill = (skill + 19 * threshold) / 20;
        }
        if (skill < -threshold)
        {
            skill = (skill + 19 * -threshold) / 20;
        }

        return Math.Max(center + skill, 0);
    }

    public static double GetAvgDefLoot(RaidApi raid)
    {
        var averageDefenseLoot = 0.0;

        var totalDefenseLoot = 0;

        var dummy_attack_count = 0.0;

        var properRaidDefenses = raid.RaidDefenses.Where(opponent => opponent.DistrictsDestroyedCount == opponent.DistrictCount);

        var defenseAttackCount = 0;

        foreach (var raidDefense in properRaidDefenses)
        {
            var oneClanLoot = 0;

            foreach (var district in raidDefense.DistrictsDestroyed)
            {
                defenseAttackCount += district.AttackCount;

                oneClanLoot += district.TotalLooted;
            }

            totalDefenseLoot += oneClanLoot;
        }

        try
        {
            var district_count = raid.RaidDefenses.FirstOrDefault().DistrictCount;

            dummy_attack_count = 3.5 * district_count;

            averageDefenseLoot = Divide(totalDefenseLoot, properRaidDefenses.Count());

            return Divide(totalDefenseLoot + averageDefenseLoot, defenseAttackCount + dummy_attack_count);
        }
        catch (NullReferenceException)
        {
            averageDefenseLoot = Divide(totalDefenseLoot, properRaidDefenses.Count());

            dummy_attack_count = 3.5 * raid.RaidDefenses.FirstOrDefault(x => x.DistrictCount != 0).DistrictCount;

            return (totalDefenseLoot + averageDefenseLoot) / (defenseAttackCount + dummy_attack_count);
        }
        catch (IndexOutOfRangeException)
        {
            return Divide(totalDefenseLoot, defenseAttackCount);
        }
    }

    public static double Divide(double a, double b)
    {
        return b != 0 ? a / b : 0;
    }

    public static void CheckThatFunc()
    {
        var raidApi = new CapitalRaidsRequest().CallApi("#YPPGCCY8").Result.RaidsInfo.First();

        var betterDefense = new DefenseApi();

        foreach (var defense in raidApi.RaidDefenses)
        {
            if (defense.AttackCount > betterDefense.AttackCount)
            {
                betterDefense = defense;
            }
        }

        var summDeadUnits = 0;

        foreach (var district in betterDefense.DistrictsDestroyed)
        {
            foreach (var attack in district.MemberAttacks)
            {
                if (attack.DestructionPercentTo < 80)
                {
                    summDeadUnits += 240;
                }
                else
                {
                    summDeadUnits += 100;
                }

            }
        }

        Console.WriteLine($"Предполагаемая награда за оборону: {summDeadUnits / 25}");

        var avgDefLoot = GetAvgDefLoot(raidApi);

        var attacksCount = 0;

        int dh1, dh2, dh3, dh4, dh5, CH9, Ch10, hz;

        dh1 = dh2 = dh3 = dh4 = dh5 = CH9 = Ch10 = hz = 0;


        foreach (var raid in raidApi.AttackedCapitals)
        {
            foreach (var district in raid.DestroyedDistricts.Where(x => x.AttackCount != 0))
            {
                switch (district.DistrictLevel)
                {
                    case 1:
                        {
                            dh1++;
                            break;
                        }
                    case 2:
                        {
                            dh2++;
                            break;
                        }
                    case 3:
                        {
                            dh3++;
                            break;
                        }
                    case 4:
                        {
                            dh4++;
                            break;
                        }
                    case 5:
                        {
                            dh5++;
                            break;
                        }
                    case 9:
                        {
                            CH9++;
                            break;
                        }
                    case 10:
                        {
                            Ch10++;
                            break;
                        }
                    default:
                        {
                            hz++;
                            break;
                        }

                }

                attacksCount += district.AttackCount;
            }
        }

        Console.WriteLine($"Забранные районы для вставки в таблицу ексель: Суммарно атак: {dh1 + dh2 + dh3 + dh4 + dh5 + CH9 + Ch10 + hz}" +
            $"\ndh1:{dh1} dh2:{dh2} dh3:{dh3} dh4:{dh4} dh5:{dh5} CH9: {CH9} CH10: {Ch10} hz: {hz}");

        var avgLoot = raidApi.CapitalTotalLoot / attacksCount;

        var result = PredictPerformanceNew(avgLoot * 300, avgLoot, avgDefLoot);

        //Console.WriteLine(1834 * 0.8 + result * 0.2);

    }

    public static void MYDEFMEDALSPREDICTOR(RaidApi raidApi)
    {
        var betterDefense = new DefenseApi();

        foreach (var defense in raidApi.RaidDefenses)
        {
            if (defense.AttackCount > betterDefense.AttackCount)
            {
                betterDefense = defense;
            }
        }

        var summDeadUnits = 0;

        foreach (var district in betterDefense.DistrictsDestroyed)
        {
            foreach (var attack in district.MemberAttacks)
            {
                if (attack.DestructionPercentTo < 80)
                {
                    summDeadUnits += 240;
                }
                else
                {
                    summDeadUnits += 100;
                }

            }
        }

        Console.WriteLine($"{summDeadUnits / 25}");
    }

    private static TelegramBotClient _client = new TelegramBotClient(token: System.IO.File.ReadAllText(@"./../../../../CustomSolutionElements/TelegramBotClientToken.txt"));

    async static Task Main(string[] args)
    {
        //CheckThatFunc();

        //CreateNewTestDb("#YPPGCCY8", "#UQQGYJJP");

        //UpdateDbCommandHandler.UpdateClanCurrentRaid("#YPPGCCY8");

        _client.StartReceiving(HandleUpdateAsync, HandleError);

        Console.WriteLine("Bot started");

        Console.ReadLine();

        await Task.CompletedTask;
    }

    async static Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                Console.Write($"{DateTime.Now}: Принято сообщение: \"{update.Message.Text}\" от ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{update.Message.Chat.Username}");
                Console.ResetColor();

                Navigation.Execute(botClient, update.Message);

                return;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Проигнорировали исключение " + e.Message + "в чате " + update.Message);
        }
    }

    static Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {

        Console.WriteLine($"Прилетело исключение {exception.Message}");

        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
            $"Ошбика телеграм API:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);

        return Task.CompletedTask;
    }

    public static void CreateNewTestDb(params string[] ClanTags)
    {
        AddToDbCommandHandler.ResetDb();

        foreach (var clanTag in ClanTags)
        {
            if (clanTag == "#VUJCUQ9Y")
            {
                AddToDbCommandHandler.AddTrackedClan(clanTag, adminsKey: "$VIKAND");
            }
            else
            {
                AddToDbCommandHandler.AddTrackedClan(clanTag, adminsKey: "$KEFamily0707");
            }

            AddToDbCommandHandler.AddClanMembers(clanTag);

            AddToDbCommandHandler.AddCurrentRaidToClan(clanTag);

            AddToDbCommandHandler.AddCurrentClanWarToClan(clanTag);

            Console.WriteLine("Clan aded");
        }
    }
}