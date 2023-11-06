using CoCApiDealer.ApiRequests;
using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Builders;
using CoCStatsTracker.Items.Exceptions;
using CoCStatsTracker.Items.Helpers;
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
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClanBuilder = new TrackedClanBuilder();

            var clanInfoFromApi = new ClanInfoRequest().CallApi(clanTag).Result;

            FailedPullFromApiException.ThrowByPredicate(() => clanInfoFromApi == null, "AddTrackedClan is failed, clan from API is null");

            AlreadyExistsException.ThrowByPredicate(() => dbContext.TrackedClans
            .Any(x => x.Tag == clanInfoFromApi.Tag), "AddTrackedClan is failed, this clan already exists");

            trackedClanBuilder.SetBaseProperties(clanInfoFromApi, adminsKey);

            dbContext.TrackedClans.Add(trackedClanBuilder.Clan);

            dbContext.SaveChanges();
        }
    }

    public static void AddClanMembers(string clanTag)
    {
        var clanInfoFromApi = new ClanInfoRequest().CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => clanInfoFromApi == null, "AddClanMembers is failed, clan from API is null");

        var clanMembersTagsFromApi = clanInfoFromApi.Members;

        var clanMembers = new List<ClanMember>();

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan.Id == 0, "AddClanMembers is failed, no such clan found");

            AlreadyExistsException.ThrowByPredicate(() => trackedClan.ClanMembers.Count > 0, "AddClanMembers is failed, clanMembers already exists");

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

            NotFoundException.ThrowByPredicate(() => trackedClan.Id == 0, "AddLastClanMembersStaticstics is failed, no such clan found");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            trackedClanBuilder.SetLastClanMembersStaticstics(trackedClanBuilder.Clan.ClanMembers);

            dbContext.SaveChanges();
        }
    }


    public static void AddCurrentRaidToClan(string clanTag)
    {
        var raidInfoFromApiresult = new CapitalRaidsRequest().CallApi(clanTag, 1).Result;

        FailedPullFromApiException.ThrowByPredicate(() => raidInfoFromApiresult == null, "AddCurrentRaidToClan is failed, Raid form API is null");

        var raidInfoFromApi = raidInfoFromApiresult.RaidsInfo.First();

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan.Id == 0, "AddCurrentRaidToClan is failed, no such clan found");

            AlreadyExistsException.ThrowByPredicate(() => trackedClan.CapitalRaids
            .Any(x => x.StartedOn == DateTimeParser.ParseToDateTime(raidInfoFromApi.StartTime).ToLocalTime()), "AddCurrentRaidToClan is failed, this raid already exists");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            var raidBuilder = new CapitalRaidBuilder();

            raidBuilder.SetBaseProperties(raidInfoFromApi);

            raidBuilder.SetTrackedClan(trackedClanBuilder.Clan);

            raidBuilder = AddRaidDefenses(raidBuilder, raidInfoFromApi);

            raidBuilder = AddRaidMembers(trackedClanBuilder, raidBuilder, raidInfoFromApi);

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


    public static void AddCurrentClanWarToClan(string clanTag)
    {
        var currentWarInfoFromApi = new CurrentWarRequest().CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => currentWarInfoFromApi == null,
            "AddCurrentClanWarToClan clan is not in classic war.");

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan.Id == 0, "AddCurrentClanWarToClan is failed, no such clan found");

            AlreadyExistsException.ThrowByPredicate(() => trackedClan.ClanWars
           .Any(x => x.StartedOn == DateTimeParser.ParseToDateTime(currentWarInfoFromApi.StartTime)), "AddCurrentClanWarToClan is failed, this war already exists");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            var clanWarBuilder = new ClanWarBuilder();

            clanWarBuilder.SetBaseProperties(currentWarInfoFromApi);

            clanWarBuilder.SetTrackedClan(trackedClanBuilder.Clan);

            clanWarBuilder = AddEnemyWarMembers(clanWarBuilder, currentWarInfoFromApi);

            clanWarBuilder = AddCwMembersWithAttacks(trackedClanBuilder, clanWarBuilder, currentWarInfoFromApi);

            trackedClanBuilder.AddClanWar(clanWarBuilder.ClanWar);

            dbContext.SaveChanges();
        }
    }

    public static void AddCwlClanWarsToClan(string clanTag)
    {
        var cwlGroupRequest = new CwlGroupRequest().CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => cwlGroupRequest == null, "AddCwlWarsToClan is failed, clan is not in CWL group");

        var cwlWarsApi = GetCwlWars(cwlGroupRequest, clanTag);

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan is { Id: 0 }, "AddCwlWarsToClan is failed, no such clan found");

            foreach (var cwlWarInfoFromApi in cwlWarsApi)
            {
                if (trackedClan.ClanWars.Any(x => x.StartedOn == cwlWarInfoFromApi.StartTime.ParseToDateTime()))
                {
                    continue;
                }

                var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

                var clanWarBuilder = new ClanWarBuilder();

                if (cwlWarInfoFromApi.OpponentResults.Tag == clanTag)
                {
                    var newClanResults = cwlWarInfoFromApi.OpponentResults;

                    var nweOpponentResults = cwlWarInfoFromApi.ClanResults;

                    cwlWarInfoFromApi.ClanResults = newClanResults;

                    cwlWarInfoFromApi.OpponentResults = nweOpponentResults;
                }

                clanWarBuilder.SetTrackedClan(trackedClanBuilder.Clan);

                clanWarBuilder.SetBaseProperties(cwlWarInfoFromApi, true, 1);

                clanWarBuilder = AddEnemyWarMembers(clanWarBuilder, cwlWarInfoFromApi);

                clanWarBuilder = AddCwMembersWithAttacks(trackedClanBuilder, clanWarBuilder, cwlWarInfoFromApi);

                trackedClanBuilder.AddClanWar(clanWarBuilder.ClanWar);
            }

            dbContext.SaveChanges();
        }
    }

    public static List<ClanWarApi> GetCwlWars(CwlGroupApi cwlGroupRequest, string clanTag)
    {
        var clanWarsApi = new List<ClanWarApi>();

        foreach (var round in cwlGroupRequest.Rounds.Where(x => x.WarTags.FirstOrDefault() != "#0"))
        {
            foreach (var warTag in round.WarTags)
            {
                var cwlWar = new CwlWarRequest().CallApi(warTag).Result;

                FailedPullFromApiException.ThrowByPredicate(() => cwlWar == null, "GetCwlWarTags is failed, bad cwlWarTag");

                if (cwlWar.ClanResults.Tag == clanTag || cwlWar.OpponentResults.Tag == clanTag)
                {
                    clanWarsApi.Add(cwlWar);
                }
            }
        }

        return clanWarsApi;
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