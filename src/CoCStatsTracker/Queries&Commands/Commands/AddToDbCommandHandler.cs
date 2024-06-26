﻿using CoCApiDealer.ApiRequests;
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
    public static void ResetDb()
    {
        using AppDbContext dbContext = new(true);

        dbContext.SaveChanges();
    }

    public static void AddTrackedClan(string clanTag)
    {
        using AppDbContext dbContext = new();

        var trackedClanBuilder = new TrackedClanBuilder();

        var clanInfoFromApi = ClanInfoRequest.CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => clanInfoFromApi == null, "AddTrackedClan is failed, clan from API is null");

        AlreadyExistsException.ThrowByPredicate(() => dbContext.TrackedClans
        .Any(x => x.Tag == clanInfoFromApi.Tag), "AddTrackedClan - is failed, this clan already exists");

        trackedClanBuilder.SetBaseProperties(clanInfoFromApi);

        dbContext.TrackedClans.Add(trackedClanBuilder.Clan);

        dbContext.SaveChanges();
    }

    public static void AddClanMembers(string clanTag)
    {
        var clanInfoFromApi = ClanInfoRequest.CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => clanInfoFromApi == null, "AddClanMembers - is failed, clan from API is null");

        var clanMembersTagsFromApi = clanInfoFromApi.Members;

        var clanMembers = new List<ClanMember>();

        using (AppDbContext dbContext = new())
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan == null || trackedClan.Id == 0, "AddClanMembers - is failed, no such clan found");

            AlreadyExistsException.ThrowByPredicate(() => trackedClan.ClanMembers.Count > 0, "AddClanMembers - is failed, clanMembers already exists");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            var SetMemberPropertyTasks = clanMembersTagsFromApi.Select(async x =>
            {
                var playerInfoFromApi = await (PlayerRequest.CallApi(x.Tag));

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
        using AppDbContext dbContext = new();

        var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClan == null || trackedClan.Id == 0, "AddLastClanMembersStaticstics - is failed, no such clan found");

        var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

        trackedClanBuilder.SetLastClanMembersStaticstics(trackedClanBuilder.Clan.ClanMembers);

        dbContext.SaveChanges();
    }


    public static void AddCurrentRaidToClan(string clanTag)
    {
        var raidInfoFromApiresult = CapitalRaidsRequest.CallApi(clanTag, 1).Result;

        FailedPullFromApiException.ThrowByPredicate(() => raidInfoFromApiresult == null, "AddCurrentRaidToClan is failed, Raid form API is null");

        var raidInfoFromApi = raidInfoFromApiresult.RaidsInfo.First();

        using AppDbContext dbContext = new();

        var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClan == null || trackedClan.Id == 0, "AddCurrentRaidToClan is failed, no such clan found");

        AlreadyExistsException.ThrowByPredicate(() => trackedClan.CapitalRaids
        .Any(x => x.StartedOn == DateTimeParser.ParseToDateTime(raidInfoFromApi.StartTime).ToLocalTime()), "AddCurrentRaidToClan is failed, this raid already exists");

        var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

        var raidBuilder = new CapitalRaidBuilder();

        raidBuilder.SetBaseProperties(raidInfoFromApi);

        raidBuilder.SetTrackedClan(trackedClanBuilder.Clan);

        raidBuilder = AddRaidDefenses(raidBuilder, raidInfoFromApi);

        raidBuilder = AddAttackedClansAndRaidMembers(trackedClanBuilder, raidBuilder, raidInfoFromApi);

        trackedClanBuilder.AddCapitalRaid(raidBuilder.Raid);

        dbContext.SaveChanges();
    }

    public static CapitalRaidBuilder AddRaidDefenses(CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var raidDefenseBuilder = new RaidDefenseBuilder();

        raidDefenseBuilder.SetBaseProperties(raidInfoFromApi.RaidDefenses);

        raidDefenseBuilder.SetRaid(raidBuilder.Raid);

        raidBuilder.SetRaidDefenses(raidDefenseBuilder.Defenses);

        return raidBuilder;
    }

    public static CapitalRaidBuilder AddAttackedClansAndRaidMembers(TrackedClanBuilder trackedClanBuilder, CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var attackedClans = new List<AttackedClanOnRaid>();
        //Создаем AttackedClans без атак
        foreach (var attackedClanApi in raidInfoFromApi.AttackedCapitals)
        {
            var attackedClanOnRaidBuilder = new AttackedClanOnRaidBuilder();

            attackedClanOnRaidBuilder.SetBaseProperties(attackedClanApi);

            var destroyedDistricts = new List<DefeatedEmemyDistrict>();

            foreach (var destroyedDistrict in attackedClanApi.DestroyedDistricts)
            {
                destroyedDistricts.Add(new DefeatedEmemyDistrict()
                {
                    Name = destroyedDistrict.Name,
                    Level = destroyedDistrict.DistrictLevel,
                    TotalDistrictLoot = destroyedDistrict.TotalLooted
                });
            }

            attackedClanOnRaidBuilder.SetDestroyedDistricts(destroyedDistricts);

            attackedClans.Add(attackedClanOnRaidBuilder.AttackedClan);
        }

        var raidMemberBuilders = new List<RaidMemberBuilder>();
        //Создаем RaidMembers без атак
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

        var alreadyAddedClanTags = new List<string>();

        //Билдим все RaidAttacks, кладем в одну кучу.
        foreach (var raidOnClanApi in raidInfoFromApi.AttackedCapitals)
        {
            foreach (var destroyedDistrictApi in raidOnClanApi.DestroyedDistricts.Where(x => x.MemberAttacks is not null))
            {
                var sortedAttacksApi = destroyedDistrictApi.MemberAttacks.OrderBy(x => x.DestructionPercentTo).ToList();

                var destructionPercentFrom = 0;

                foreach (var memberAttackOnDistrictApi in sortedAttacksApi)
                {
                    var raidAttackBuilder = new RaidAttackBuilder();

                    var attackedClan = new AttackedClanOnRaid();

                    if (alreadyAddedClanTags.Contains(raidOnClanApi.DefenderClan.Tag))
                    {
                        attackedClan = attackedClans.Last(x => x.Tag == raidOnClanApi.DefenderClan.Tag);
                    }
                    else
                    {
                        attackedClan = attackedClans.FirstOrDefault(x => x.Tag == raidOnClanApi.DefenderClan.Tag);
                    }

                    var destroyedDistrict = attackedClan.DefeatedEmemyDistricts.FirstOrDefault(x => x.Name == destroyedDistrictApi.Name);

                    raidAttackBuilder.SetBaseProperties(destructionPercentFrom, memberAttackOnDistrictApi.DestructionPercentTo);

                    raidAttackBuilder.SetDefeatedDistrict(destroyedDistrict);

                    raidAttackBuilder.SetAttackedClan(attackedClan);

                    raidAttackBuilder.SetRaidMember(raidMemberBuilders
                        .FirstOrDefault(x => x.Member.Tag == memberAttackOnDistrictApi.Attacker.Tag).Member);

                    allRaidAttacks.Add(raidAttackBuilder.RaidAttack);

                    destructionPercentFrom = memberAttackOnDistrictApi.DestructionPercentTo;
                }
            }

            alreadyAddedClanTags.Add(raidOnClanApi.DefenderClan.Tag);
        }

        //Присваиваем каждому рейд мемберу полагающиеся атаки.
        foreach (var raidMemberBuilder in raidMemberBuilders)
        {
            raidMemberBuilder.SetRaidMemberAttacks(allRaidAttacks
                .Where(x => x.RaidMember.Tag == raidMemberBuilder.Member.Tag).ToList());
        }

        raidBuilder.SetAttackedClans(attackedClans);

        raidBuilder.SetRaidMembers(raidMemberBuilders
            .Select(rmb => rmb.Member)
            .ToList());

        return raidBuilder;
    }


    public static void AddCurrentClanWarToClan(string clanTag)
    {
        var currentWarInfoFromApi = CurrentWarRequest.CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => currentWarInfoFromApi == null,
            "AddCurrentClanWarToClan clan is not in classic war.");

        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null || trackedClanDb.Id == 0, "AddCurrentClanWarToClan is failed, no such clan found");

        AlreadyExistsException.ThrowByPredicate(() => trackedClanDb.ClanWars
       .Any(x => x.StartedOn == DateTimeParser.ParseToDateTime(currentWarInfoFromApi.StartTime)), "AddCurrentClanWarToClan is failed, this war already exists");

        var trackedClanBuilder = new TrackedClanBuilder(trackedClanDb);

        var clanWarBuilder = new ClanWarBuilder();

        clanWarBuilder.SetBaseProperties(currentWarInfoFromApi);

        clanWarBuilder.SetOpponentWarStatistics(ClanInfoRequest.CallApi(clanTag).Result);

        clanWarBuilder.SetTrackedClan(trackedClanBuilder.Clan);

        clanWarBuilder = AddEnemyWarMembers(clanWarBuilder, currentWarInfoFromApi);

        clanWarBuilder = AddCwMembersWithAttacks(trackedClanBuilder, clanWarBuilder, currentWarInfoFromApi);

        trackedClanBuilder.AddClanWar(clanWarBuilder.ClanWar);

        dbContext.SaveChanges();
    }

    public static void AddCurrentCwlClanWarsToClan(string clanTag)
    {
        var cwlGroupRequest = CwlGroupRequest.CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => cwlGroupRequest == null, "AddCwlWarsToClan is failed, clan is not in CWL group");

        var cwlWarsApi = GetCwlWars(cwlGroupRequest, clanTag);

        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null || trackedClanDb.Id == 0, "AddCwlWarsToClan is failed, no such clan found");

        foreach (var cwlWarInfoFromApi in cwlWarsApi)
        {
            if (trackedClanDb.ClanWars.Any(x => x.StartedOn == cwlWarInfoFromApi.StartTime.ParseToDateTime()))
            {
                continue;
            }

            var trackedClanBuilder = new TrackedClanBuilder(trackedClanDb);

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

            clanWarBuilder.SetOpponentWarStatistics(ClanInfoRequest.CallApi(clanTag).Result);

            clanWarBuilder = AddEnemyWarMembers(clanWarBuilder, cwlWarInfoFromApi);

            clanWarBuilder = AddCwMembersWithAttacks(trackedClanBuilder, clanWarBuilder, cwlWarInfoFromApi);

            trackedClanBuilder.AddClanWar(clanWarBuilder.ClanWar);
        }

        dbContext.SaveChanges();
    }

    public static List<ClanWarApi> GetCwlWars(CwlGroupApi cwlGroupRequest, string clanTag)
    {
        var clanWarsApi = new List<ClanWarApi>();

        foreach (var round in cwlGroupRequest.Rounds.Where(x => x.WarTags.FirstOrDefault() != "#0"))
        {
            foreach (var warTag in round.WarTags)
            {
                var cwlWar = CwlWarRequest.CallApi(warTag).Result;

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

            clanMemberOnWar?.WarMemberships.Add(warMemberBuilder.WarMember);

            warMembers.Add(warMemberBuilder.WarMember);
        }

        clanWarBuilder.SetWarMembers(warMembers);

        return clanWarBuilder;
    }
}