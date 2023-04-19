using CoCApiDealer.ApiRequests;
using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Builders;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Storage;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    /// <summary>
    /// #YPPGCCY8 - тег клана
    /// #2VGG92CL9 - тег игрока
    /// </summary>

    static async Task Main(string[] args)
    {
        //Подтянули информацию о клане, создали объект домейнного типа
        var clanInfoFromApi = new ClanInfoRequest().CallApi("#YPPGCCY8").Result;

        var trackedClanBuilder = new TrackedClanBuilder();

        trackedClanBuilder.SetBaseProperties(clanInfoFromApi);


        //Сделали запрос к апи ручке player и заполнили данными наш трекед клан
        var clanMembers = new List<ClanMember>();

        foreach (var member in clanInfoFromApi.Members)
        {
            var playerInfoFromApi = new PlayerRequest().CallApi(member.Tag).Result;

            var clanMemberBuilder = new ClanMemberBuilder();
            clanMemberBuilder.SetBaseProperties(playerInfoFromApi);
            clanMemberBuilder.SetUnits(playerInfoFromApi.Troops, playerInfoFromApi.Heroes);

            clanMembers.Add(clanMemberBuilder.ClanMember);
        }

        trackedClanBuilder.SetClanMembers(clanMembers);

        //Подтянули информацию о последнем рейде, создали домейнный объект со всеми связями
        var raidInfoFromApi = new CapitalRaidsRequest().CallApi("#YPPGCCY8", 1).Result.RaidsInfo.First();

        var raidBuilder = new CapitalRaidBuilder();

        raidBuilder.SetBaseProperties(raidInfoFromApi);

        //Добавляем информацию о защитах
        var raidDefenseBuilder = new RaidDefenseBuilder();

        raidDefenseBuilder.SetBaseProperties(raidInfoFromApi.RaidDefenses);

        raidBuilder.SetRaidDefenses(raidDefenseBuilder.Defenses);


        //Добавляем информацию о побежденных кланах и рейд атаках

        var defeatedClans = new List<DefeatedClan>();
        var raidAttacks = new List<RaidAttack>();

        foreach (var defeatedClanApi in raidInfoFromApi.RaidOnClans)
        {
            var defeatedClanBuilder = new DefeatedClanBuilder();
            defeatedClanBuilder.SetBaseProperties(defeatedClanApi);

            var destroyedDistricts = new List<OpponentDistrict>();

            foreach (var defeatedDistrict in defeatedClanApi.DistrictsDestroyed)
            {
                var defeatedDistrictBuilder = new OpponentDistrictBuilder();
                defeatedDistrictBuilder.SetBaseProperties(defeatedDistrict);

                if (defeatedDistrict.MemberAttacks != null)
                {
                    foreach (var attack in defeatedDistrict.MemberAttacks)
                    {
                        var raidAttackBuilder = new RaidAttackBuilder();
                        raidAttackBuilder.SetBaseProperties(attack);

                        raidAttacks.Add(raidAttackBuilder.RaidAttack);
                        raidAttacks.Last().OpponentDistrict = defeatedDistrictBuilder.District;
                    }
                }

                destroyedDistricts.Add(defeatedDistrictBuilder.District);
            }

            defeatedClanBuilder.SetOpponentDistricts(destroyedDistricts);

            defeatedClans.Add(defeatedClanBuilder.Clan);
        }

        raidBuilder.SetDefeatedClans(defeatedClans);
        raidBuilder.SetAttacks(raidAttacks);

        // Добавляем RaidMembers-ов в клан и в игроков

        var members = new List<RaidMember>();

        foreach (var member in raidInfoFromApi.RaidMembers)
        {
            var raidMemberBuilder = new RaidMemberBuilder();
            raidMemberBuilder.SetBaseProperties(member);
            members.Add(raidMemberBuilder.Member);
        }

        foreach (var member in trackedClanBuilder.TrackedClan.ClanMembers)
        {
            var clanMemberBuilder = new ClanMemberBuilder(member);
            if (members.FirstOrDefault(x => x.Tag == member.Tag) != null)
            {
                clanMemberBuilder.AddRaidMembership(members.FirstOrDefault(x => x.Tag == member.Tag));
            }

        }

        raidBuilder.SetRaidMembers(members);

        //Добавляем информацию об этом рейде в клан и в игроков
        trackedClanBuilder.AddCapitalRaid(raidBuilder.Raid);

        Console.WriteLine("Api mapping etc. compleeted");

        RunDb(clanMembers, trackedClanBuilder.TrackedClan, raidBuilder.Raid);
    }

    static void RunDb(List<ClanMember> members, TrackedClan clan, CapitalRaid raid)
    {
        using (AppDbContext db = new AppDbContext("Data Source=CoCStatsTracker.db"))
        {


            db.TrackedClans.Add(clan);

            db.Complete();

            Console.WriteLine("Объекты успешно сохранены");

            // получаем объекты из бд и выводим на консоль
            var users = db.ClanMembers.ToList();

            Console.WriteLine("Список объектов:");

            foreach (var user in users)
            {
                Console.WriteLine($"{user.Id}.{user.Name} - {user.WarStars} - {user.RaidMembership}");
            }
        }
    }

}
