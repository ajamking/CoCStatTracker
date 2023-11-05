using CoCApiDealer.ApiRequests;
using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Builders;
using CoCStatsTracker.Items.Exceptions;
using Domain.Entities;
using Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoCStatsTracker;

public static class AddToDbCommandHandler
{
    private static string _dbConnectionString = "Data Source=./../../../../CustomSolutionElements/CoCStatsTracker.db";

    public static void SetConnectionString(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
    }

    public static void ResetDb()
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString, true))
        {
            dbContext.SaveChanges();
        }
    }

    public static void AddTrackedClan(string clanTag, string adminsKey)
    {
        var trackedClanBuilder = new TrackedClanBuilder();

        var clanInfoFromApi = new ClanInfoRequest().CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => clanInfoFromApi is { Tag: null }, "AddTrackedClan is failed, bad API responce");

        trackedClanBuilder.SetBaseProperties(clanInfoFromApi, adminsKey);

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            dbContext.TrackedClans.Add(trackedClanBuilder.Clan);

            dbContext.SaveChanges();
        }
    }

    public static void AddClanMembers(string clanTag)
    {
        var clanMembersTagsFromApi = new ClanInfoRequest().CallApi(clanTag).Result.Members;

        FailedPullFromApiException.ThrowByPredicate(() => clanMembersTagsFromApi is { Length: 0 }, "AddClanMembers is failed, bad API responce");

        var clanMembers = new List<ClanMember>();

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            FailedPullFromApiException.ThrowByPredicate(() => trackedClan is { Id: 0 }, "AddClanMembers is failed, no such clan found");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            var SetMemberPropertyTasks = clanMembersTagsFromApi.Select(async x =>
            {
                var playerInfoFromApi = await (new PlayerRequest().CallApi(x.Tag));

                var clanMemberBuilder = new ClanMemberBuilder();

                clanMemberBuilder.SetBaseProperties(playerInfoFromApi);

                clanMemberBuilder.SetUnits(playerInfoFromApi.Troops, playerInfoFromApi.Heroes);

                clanMemberBuilder.SetTrackedClan(trackedClanBuilder.Clan);

                clanMembers.Add(clanMemberBuilder.ClanMember);
            }).ToList();

            Task.WhenAll(SetMemberPropertyTasks).GetAwaiter().GetResult();

            trackedClanBuilder.SetClanMembers(clanMembers);

            dbContext.SaveChanges();
        }

        AddLastClanMembersStaticstics(clanTag);
    }

    private static void AddLastClanMembersStaticstics(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            FailedPullFromApiException.ThrowByPredicate(() => trackedClan is { Id: 0 }, "AddLastClanMembersStaticstics is failed, no such clan found");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            trackedClanBuilder.SetLastClanMembersStaticstics(trackedClanBuilder.Clan.ClanMembers);

            dbContext.SaveChanges();
        }
    }


    public static void AddCurrentRaidToClan(string clanTag)
    {
        var raidInfoFromApi = new CapitalRaidsRequest().CallApi(clanTag, 1).Result.RaidsInfo.First();

        FailedPullFromApiException.ThrowByPredicate(() => raidInfoFromApi is null, "AddCurrentRaidToClan is failed, bad API responce");

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan is { Id: 0 }, "AddCurrentRaidToClan is failed, no such clan found");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            var raidBuilder = new CapitalRaidBuilder();

            raidBuilder.SetBaseProperties(raidInfoFromApi);

            raidBuilder.SetTrackedClan(trackedClanBuilder.Clan);

            raidBuilder = AddRaidDefenses(raidBuilder, raidInfoFromApi);

            raidBuilder = AddRaidMembers(trackedClanBuilder, raidBuilder, raidInfoFromApi);

            AlreadyExistsException.ThrowByPredicate(() => trackedClanBuilder.Clan.CapitalRaids.Any(x => x.StartedOn == raidBuilder.Raid.StartedOn), "AddCurrentRaidToClan is failed, this raid already exists");

            trackedClanBuilder.AddCapitalRaid(raidBuilder.Raid);

            dbContext.SaveChanges();
        }
    }

    private static CapitalRaidBuilder AddRaidDefenses(CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var raidDefenseBuilder = new RaidDefenseBuilder();

        raidDefenseBuilder.SetBaseProperties(raidInfoFromApi.RaidDefenses);

        raidDefenseBuilder.SetRaid(raidBuilder.Raid);

        raidBuilder.SetRaidDefenses(raidDefenseBuilder.Defenses);

        return raidBuilder;
    }

    private static CapitalRaidBuilder AddRaidMembers(TrackedClanBuilder trackedClanBuilder, CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var raidMemberBuilders = new List<RaidMemberBuilder>();

        //Создаем рейд мемберов без атак
        foreach (var raidMemberApi in raidInfoFromApi.RaidMembers)
        {
            var raidMemberBuilder = new RaidMemberBuilder();

            raidMemberBuilder.SetBaseProperties(raidMemberApi);

            raidMemberBuilder.SetRaid(raidBuilder.Raid);

            raidMemberBuilder.SetClanMember(trackedClanBuilder.Clan.ClanMembers
                .FirstOrDefault(x => x.Tag == raidMemberApi.Tag));

            raidMemberBuilders.Add(raidMemberBuilder);
        }

        var allRaidAttacks = new List<RaidAttack>();

        //Билдим все RaidAttacks, кладем в одну кучу.
        foreach (var raidOnClanApi in raidInfoFromApi.RaidOnClans)
        {
            foreach (var destroyedDistrictApi in raidOnClanApi.DestroyedDistricts.Where(x => x.MemberAttacks is not null))
            {
                var sortedAttacksApi = destroyedDistrictApi.MemberAttacks.OrderBy(x => x.DestructionPercentTo).ToList();

                var destructionPercentFrom = 0;

                foreach (var memberAttackApi in sortedAttacksApi)
                {
                    var raidAttackBuilder = new RaidAttackBuilder();

                    raidAttackBuilder.SetBaseProperties(memberAttackApi, raidOnClanApi, destroyedDistrictApi, destructionPercentFrom);

                    destructionPercentFrom = memberAttackApi.DestructionPercentTo;

                    raidAttackBuilder.SetRaidMember(raidMemberBuilders.FirstOrDefault(x => x.Member.MemberTag == memberAttackApi.Attacker.Tag).Member);

                    allRaidAttacks.Add(raidAttackBuilder.RaidAttack);
                }
            }
        }

        var raidMembers = new List<RaidMember>();

        //Раскидываем атаки по рейдМемберам
        foreach (var rmb in raidMemberBuilders)
        {
            var memberRaidAttacks = new List<RaidAttack>();

            foreach (var attack in allRaidAttacks)
            {
                if (attack.MemberTag == rmb.Member.MemberTag)
                {
                    memberRaidAttacks.Add(attack);
                }
            }

            rmb.SetRaidMemberAttacks(memberRaidAttacks);

            raidMembers.Add(rmb.Member);
        }

        raidBuilder.SetRaidMembers(raidMembers);

        return raidBuilder;
    }


    public static void AddCurrentClanWarToClan(string clanTag, bool isCwLWar = false, string cwlWarTag = "")
    {
        var clanWarInfoFromApi = isCwLWar ?
               new CwlWarRequest().CallApi(cwlWarTag).Result :
               new CurrentWarRequest().CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => clanWarInfoFromApi is { StartTime: null }, "AddCurrentClanWarToClan is failed, bad API responce");

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan is { Id: 0 }, "AddCurrentClanWarToClan is failed, no such clan found");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            var clanWarBuilder = new ClanWarBuilder();

            clanWarBuilder.SetBaseProperties(clanWarInfoFromApi);

            clanWarBuilder.SetTrackedClan(trackedClanBuilder.Clan);

            clanWarBuilder = AddEnemyWarMembers(clanWarBuilder, clanWarInfoFromApi);

            clanWarBuilder = AddCwMembersWithAttacks(trackedClanBuilder, clanWarBuilder, clanWarInfoFromApi);

            AlreadyExistsException.ThrowByPredicate(() => trackedClanBuilder.Clan.ClanWars.Any(x => x.StartedOn == clanWarBuilder.ClanWar.StartedOn), "AddCurrentClanWarToClan is failed, this CW already exists");
            
            trackedClanBuilder.AddClanWar(clanWarBuilder.ClanWar);

            dbContext.SaveChanges();
        }
    }

    private static ClanWarBuilder AddEnemyWarMembers(ClanWarBuilder clanWarBuilder, ClanWarApi clanWarInfoFromApi)
    {
        var enemyWarmembers = new List<EnemyWarMember>();

        foreach (var enemyWarMember in clanWarInfoFromApi.OpponentResults.WarMembers)
        {
            var enemyWarMemberBuilder = new EnemyWarMemberBuilder();

            enemyWarMemberBuilder.SetBaseProperties(enemyWarMember);

            enemyWarMemberBuilder.SetClanWar(clanWarBuilder.ClanWar);

            enemyWarmembers.Add(enemyWarMemberBuilder.EnemyWarMember);
        }

        clanWarBuilder.SetEnemyWarMembers(enemyWarmembers);

        return clanWarBuilder;
    }

    private static ClanWarBuilder AddCwMembersWithAttacks(TrackedClanBuilder trackedClanBuilder, ClanWarBuilder clanWarBuilder, ClanWarApi clanWarInfoFromApi)
    {
        var warMembers = new List<WarMember>();

        foreach (var warMemberApi in clanWarInfoFromApi.ClanResults.WarMembers)
        {
            var warMemberBuilder = new WarMemberBuilder();

            warMemberBuilder.SetBaseProperties(warMemberApi);

            var warMemberAttacks = new List<WarAttack>();

            if (warMemberApi.Attacks != null)
            {
                foreach (var warAttack in warMemberApi.Attacks)
                {
                    var warAttackBuilder = new WarAttackBuilder();

                    warAttackBuilder.SetBaseProperties(warAttack);

                    warAttackBuilder.SetWarMember(warMemberBuilder.WarMember);

                    warAttackBuilder.SetEnemyWarMember(clanWarBuilder.ClanWar.EnemyWarMembers
                        .First(x => x.Tag == warAttack.DefenderTag));

                    warMemberAttacks.Add(warAttackBuilder.WarAttack);
                }
            }

            warMemberBuilder.SetWarAttacks(warMemberAttacks);

            warMemberBuilder.SetClanWar(clanWarBuilder.ClanWar);

            var clanMemberOnWar = trackedClanBuilder.Clan.ClanMembers
               .FirstOrDefault(x => x.Tag == warMemberBuilder.WarMember.Tag);

            warMemberBuilder.SetClanMember(clanMemberOnWar);

            if (clanMemberOnWar != null)
            {
                clanMemberOnWar.WarMemberships.Add(warMemberBuilder.WarMember);
            }

            warMembers.Add(warMemberBuilder.WarMember);
        }

        clanWarBuilder.SetWarMembers(warMembers);

        return clanWarBuilder;
    }

}
