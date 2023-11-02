using CoCApiDealer.ApiRequests;
using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Builders;
using CoCStatsTracker.Items.Exceptions;
using Domain.Entities;
using Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoCStatsTracker.Items.Helpers;

namespace CoCStatsTracker;

public static class UpdateDbCommandHandler
{
    private static string _dbConnectionString = "Data Source=./../../../../CustomSolutionElements/CoCStatsTracker.db";

    public static void SetConnectionString(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
    }

    public static void UpdateTrackedClanBaseProperties(string clanTag, string adminsKey)
    {
        var clanInfoFromApi = new ClanInfoRequest().CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => clanInfoFromApi is { Tag: null }, "UpdateTrackedClanBaseProperties is failed, bad API responce");

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan is { Id: 0 }, "UpdateTrackedClanBaseProperties is failed, no such clan found");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            trackedClanBuilder.SetBaseProperties(clanInfoFromApi, adminsKey);

            dbContext.SaveChanges();
        }
    }

    public static void UpdateTrackedClanClanMembers(string clanTag)
    {
        var clanMembersTagsFromApi = new ClanInfoRequest().CallApi(clanTag).Result.Members;

        FailedPullFromApiException.ThrowByPredicate(() => clanMembersTagsFromApi is { Length: 0 }, "UpdateTrackedClanClanMembers is failed, bad API responce");

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan is { Id: 0 }, "UpdateTrackedClanClanMembers is failed, no such clan found");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            var updatedClanMembers = new List<ClanMember>();

            var SetMemberPropertyTasks = clanMembersTagsFromApi.Select(async x =>
            {
                var playerInfoFromApi = await (new PlayerRequest().CallApi(x.Tag));

                var oldClanMember = trackedClanBuilder.Clan.ClanMembers
              .FirstOrDefault(x => x.Tag == playerInfoFromApi.Tag);

                FailedPullFromApiException.ThrowByPredicate(() => playerInfoFromApi is { Tag: "" }, "UpdateTrackedClanClanMembers is failed, bad API responce");

                var clanMemberBuilder = new ClanMemberBuilder(oldClanMember);

                clanMemberBuilder.SetBaseProperties(playerInfoFromApi);

                clanMemberBuilder.SetUnits(playerInfoFromApi.Troops, playerInfoFromApi.Heroes);

                clanMemberBuilder.SetTrackedClan(trackedClanBuilder.Clan);

                updatedClanMembers.Add(clanMemberBuilder.ClanMember);
            }).ToList();

            Task.WhenAll(SetMemberPropertyTasks).GetAwaiter().GetResult();

            trackedClanBuilder.SetClanMembers(updatedClanMembers);

            dbContext.SaveChanges();
        }
    }

    public static void UpdateLastClanMembersStaticstics(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            FailedPullFromApiException.ThrowByPredicate(() => trackedClan is { Id: 0 }, "UpdateLastClanMembersStaticstics is failed, no such clan found");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            trackedClanBuilder.SetLastClanMembersStaticstics(trackedClanBuilder.Clan.ClanMembers);
        }
    }


    public static void UpdateClanCurrentRaid(string clanTag)
    {
        var raidInfoFromApi = new CapitalRaidsRequest().CallApi(clanTag, 1).Result.RaidsInfo.First();

        FailedPullFromApiException.ThrowByPredicate(() => raidInfoFromApi is { StartTime: null }, "UpdateClanCurrentRaidOld is failed, bad API responce");

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan is { Id: 0 }, "UpdateClanCurrentRaidOld is failed, no such clan found");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            var raidStartedOn = DateTimeParser.Parse(raidInfoFromApi.StartTime);

            var existingCurrentRaid = trackedClanBuilder.Clan.CapitalRaids
                .FirstOrDefault(x => x.StartedOn == raidStartedOn);

            var raidBuilder = new CapitalRaidBuilder(existingCurrentRaid);

            raidBuilder.SetBaseProperties(raidInfoFromApi);

            raidBuilder.SetTrackedClan(trackedClanBuilder.Clan);

            dbContext.RaidDefenses.RemoveRange(dbContext.RaidDefenses.Where(x => x.CapitalRaid.TrackedClan.Tag == clanTag));

            raidBuilder = UpdateRadeDefences(raidBuilder, raidInfoFromApi);

            dbContext.RaidAttacks.RemoveRange(dbContext.RaidAttacks.Where(x => x.RaidMember.Raid.TrackedClan.Tag == clanTag));

            raidBuilder = UpdateRaidMembers(trackedClanBuilder, raidBuilder, raidInfoFromApi);

            dbContext.SaveChanges();
        }

    }

    private static CapitalRaidBuilder UpdateRadeDefences(CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var existingRaidDefences = raidBuilder.Raid.RaidDefenses;

        var raidDefenseBuilder = new RaidDefenseBuilder(existingRaidDefences);

        raidDefenseBuilder.SetBaseProperties(raidInfoFromApi.RaidDefenses);

        raidDefenseBuilder.SetRaid(raidBuilder.Raid);

        raidBuilder.SetRaidDefenses(raidDefenseBuilder.Defenses);

        return raidBuilder;
    }

    private static CapitalRaidBuilder UpdateRaidMembers(TrackedClanBuilder trackedClanBuilder, CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var raidMemberBuilders = new List<RaidMemberBuilder>();

        //Создаем рейд мемберов без атак
        foreach (var raidMemberApi in raidInfoFromApi.RaidMembers)
        {
            var existingRaidMember = raidBuilder.Raid.RaidMembers
             .FirstOrDefault(x => x.MemberTag == raidMemberApi.Tag);

            var raidMemberBuilder = new RaidMemberBuilder(existingRaidMember);

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
                    var existingRaidMember = raidBuilder.Raid.RaidMembers
                        .FirstOrDefault(x => x.MemberTag == memberAttackApi.Attacker.Tag);

                    var existingRaidMembersAttack = new RaidAttack();

                    foreach (var existingAttack in existingRaidMember.Attacks)
                    {
                        if (existingAttack.OpponentClanTag == raidOnClanApi.DefenderClan.Tag &&
                            existingAttack.OpponentDistrictName == destroyedDistrictApi.Name &&
                            existingAttack.DestructionPercentTo == memberAttackApi.DestructionPercentTo)
                        {
                            existingRaidMembersAttack = existingAttack;
                        }
                    }

                    var raidAttackBuilder = new RaidAttackBuilder(existingRaidMembersAttack);

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


    public static void UpdateCurrentClanWar(string clanTag, bool isCwLWar = false, string cwlWarTag = "")
    {
        var clanWarInfoFromApi = isCwLWar ?
               new CwlWarRequest().CallApi(cwlWarTag).Result :
               new CurrentWarRequest().CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => clanWarInfoFromApi is { StartTime: null }, "UpdateCurrentClanWarToClan is failed, bad API responce");

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan is { Id: 0 }, "UpdateCurrentClanWarToClan is failed, no such clan found");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            var cwStartedOn = DateTimeParser.Parse(clanWarInfoFromApi.StartTime);

            var existingCurrentCw = trackedClanBuilder.Clan.ClanWars.
                FirstOrDefault(x => x.StartedOn == cwStartedOn);

            var clanWarBuilder = new ClanWarBuilder(existingCurrentCw);

            clanWarBuilder.SetBaseProperties(clanWarInfoFromApi);

            clanWarBuilder.SetTrackedClan(trackedClanBuilder.Clan);

            clanWarBuilder = UpdateEnemyWarMembers(clanWarBuilder, clanWarInfoFromApi);

            clanWarBuilder = UpdateCwMembersWithAttacks(trackedClanBuilder, clanWarBuilder, clanWarInfoFromApi);

            dbContext.SaveChanges();
        }
    }

    private static ClanWarBuilder UpdateEnemyWarMembers(ClanWarBuilder clanWarBuilder, ClanWarApi clanWarInfoFromApi)
    {
        var enemyWarmembers = new List<EnemyWarMember>();

        foreach (var enemyWarMember in clanWarInfoFromApi.OpponentResults.WarMembers)
        {
            var existingEnemuWarMember = clanWarBuilder.ClanWar.EnemyWarMembers
               .FirstOrDefault(x => x.Tag == enemyWarMember.Tag);

            var enemyWarMemberBuilder = new EnemyWarMemberBuilder(existingEnemuWarMember);

            enemyWarMemberBuilder.SetBaseProperties(enemyWarMember);

            enemyWarMemberBuilder.SetClanWar(clanWarBuilder.ClanWar);

            enemyWarmembers.Add(enemyWarMemberBuilder.EnemyWarMember);
        }

        clanWarBuilder.SetEnemyWarMembers(enemyWarmembers);

        return clanWarBuilder;
    }

    private static ClanWarBuilder UpdateCwMembersWithAttacks(TrackedClanBuilder trackedClanBuilder, ClanWarBuilder clanWarBuilder, ClanWarApi clanWarInfoFromApi)
    {
        var warMembers = new List<WarMember>();

        foreach (var warMemberApi in clanWarInfoFromApi.ClanResults.WarMembers)
        {
            var existingWarMember = clanWarBuilder.ClanWar.WarMembers
                .FirstOrDefault(x => x.Tag == warMemberApi.Tag);

            var warMemberBuilder = new WarMemberBuilder(existingWarMember);

            warMemberBuilder.SetBaseProperties(warMemberApi);

            var newWarMemberAttacks = new List<WarAttack>();

            if (warMemberApi.Attacks is not null)
            {
                foreach (var warAttack in warMemberApi.Attacks)
                {
                    var warAttackBuilder = new WarAttackBuilder(existingWarMember.WarAttacks.FirstOrDefault(x => x.AttackOrder == warAttack.Order));

                    warAttackBuilder.SetBaseProperties(warAttack);

                    warAttackBuilder.SetWarMember(warMemberBuilder.WarMember);

                    warAttackBuilder.SetEnemyWarMember(clanWarBuilder.ClanWar.EnemyWarMembers
                        .First(x => x.Tag == warAttack.DefenderTag));

                    newWarMemberAttacks.Add(warAttackBuilder.WarAttack);
                }
            }

            warMemberBuilder.SetWarAttacks(newWarMemberAttacks);

            warMemberBuilder.SetClanWar(clanWarBuilder.ClanWar);

            var clanMemberOnWar = trackedClanBuilder.Clan.ClanMembers
               .FirstOrDefault(x => x.Tag == warMemberBuilder.WarMember.Tag);

            warMemberBuilder.SetClanMember(clanMemberOnWar);

            if (clanMemberOnWar is not null)
            {
                clanMemberOnWar.WarMemberships.Add(warMemberBuilder.WarMember);
            }

            warMembers.Add(warMemberBuilder.WarMember);
        }

        clanWarBuilder.SetWarMembers(warMembers);

        return clanWarBuilder;
    }
}
