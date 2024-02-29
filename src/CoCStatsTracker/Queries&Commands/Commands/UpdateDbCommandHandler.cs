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

public static class UpdateDbCommandHandler
{
    public static void UpdateTrackedClanBaseProperties(string clanTag)
    {
        var clanInfoFromApi = ClanInfoRequest.CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => clanInfoFromApi == null, "UpdateTrackedClanBaseProperties - is failed, Clan from API is null");

        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null || trackedClanDb.Id == 0, "UpdateTrackedClanBaseProperties - is failed, no such clan found");

        var trackedClanBuilder = new TrackedClanBuilder(trackedClanDb);

        trackedClanBuilder.SetBaseProperties(clanInfoFromApi);

        dbContext.SaveChanges();
    }

    public static void UpdateTrackedClanClanMembers(string clanTag)
    {
        var clanInfoFromApi = ClanInfoRequest.CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => clanInfoFromApi == null, "UpdateTrackedClanClanMembers - is failed, Clan from API is null");

        var clanMembersTagsFromApi = clanInfoFromApi.Members;

        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null || trackedClanDb.Id == 0, "UpdateTrackedClanClanMembers - is failed, no such clan found");

        var trackedClanBuilder = new TrackedClanBuilder(trackedClanDb);

        var updatedClanMembers = new List<ClanMember>();

        var SetMemberPropertyTasks = clanMembersTagsFromApi.Select(async x =>
        {
            var playerInfoFromApi = await (PlayerRequest.CallApi(x.Tag));

            FailedPullFromApiException.ThrowByPredicate(() => playerInfoFromApi == null, "UpdateTrackedClanClanMembers - is failed, bad API responce");

            var oldClanMemberDb = trackedClanBuilder.Clan.ClanMembers.FirstOrDefault(x => x.Tag == playerInfoFromApi.Tag);

            if (oldClanMemberDb != null && dbContext.Units.Where(x => x.ClanMemberId == oldClanMemberDb.Id).Any())
            {
                dbContext.Units.RemoveRange(dbContext.Units.Where(x => x.ClanMemberId == oldClanMemberDb.Id));
            }

            var clanMemberBuilder = new ClanMemberBuilder(oldClanMemberDb);

            clanMemberBuilder.SetBaseProperties(playerInfoFromApi);

            clanMemberBuilder.SetUnits(playerInfoFromApi.Troops, playerInfoFromApi.Heroes);

            clanMemberBuilder.SetTrackedClan(trackedClanBuilder.Clan);

            updatedClanMembers.Add(clanMemberBuilder.ClanMember);

        }).ToList();

        Task.WhenAll(SetMemberPropertyTasks).GetAwaiter().GetResult();

        trackedClanBuilder.SetClanMembers(updatedClanMembers);

        var obsoleteMembersWithoutClan = dbContext.ClanMembers.Where(x => x.TrackedClanId == null);

        dbContext.ClanMembers.RemoveRange(obsoleteMembersWithoutClan);

        dbContext.SaveChanges();

        var a = 1;
    }

    public static void ResetLastClanMembersStaticstics(string clanTag)
    {
        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null || trackedClanDb.Id == 0, "UpdateLastClanMembersStaticstics - is failed, no such clan found");

        var trackedClanBuilder = new TrackedClanBuilder(trackedClanDb);

        dbContext.PreviousClanMembers.RemoveRange(trackedClanDb.PreviousClanMembersStaticstics);

        trackedClanBuilder.SetLastClanMembersStaticstics(trackedClanBuilder.Clan.ClanMembers);

        dbContext.SaveChanges();
    }


    public static void UpdateClanCurrentRaid(string clanTag)
    {
        var raidInfoFromApiresult = CapitalRaidsRequest.CallApi(clanTag, 1).Result;

        FailedPullFromApiException.ThrowByPredicate(() => raidInfoFromApiresult == null, "UpdateClanCurrentRaid - is failed, Raid form API is null");

        var raidInfoFromApi = raidInfoFromApiresult.RaidsInfo.First();

        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null || trackedClanDb.Id == 0, "UpdateClanCurrentRaidOld - is failed, no such clan found");

        var trackedClanBuilder = new TrackedClanBuilder(trackedClanDb);

        var raidStartedOn = DateTimeParser.ParseToDateTime(raidInfoFromApi.StartTime);

        var existingCurrentRaid = trackedClanBuilder.Clan.CapitalRaids.FirstOrDefault(x => x.StartedOn == raidStartedOn);

        NotFoundException.ThrowByPredicate(() => existingCurrentRaid == null, "UpdateClanCurrentRaidOld is failed, no last raid found");

        dbContext.CapitalRaids.Remove(existingCurrentRaid);

        var raidBuilder = new CapitalRaidBuilder();

        raidBuilder.SetBaseProperties(raidInfoFromApi);

        raidBuilder.SetTrackedClan(trackedClanBuilder.Clan);

        raidBuilder = AddToDbCommandHandler.AddRaidDefenses(raidBuilder, raidInfoFromApi);

        raidBuilder = AddToDbCommandHandler.AddAttackedClansAndRaidMembers(trackedClanBuilder, raidBuilder, raidInfoFromApi);

        trackedClanBuilder.AddCapitalRaid(raidBuilder.Raid);

        dbContext.SaveChanges();
    }


    public static void UpdateCurrentClanWar(string clanTag)
    {
        var clanWarInfoFromApi = CurrentWarRequest.CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => clanWarInfoFromApi == null,
            "UpdateCurrentClanWar clan is not in classic war.");

        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null || trackedClanDb.Id == 0, "UpdateCurrentClanWarToClan - is failed, no such clan found");

        var trackedClanBuilder = new TrackedClanBuilder(trackedClanDb);

        var cwStartedOn = DateTimeParser.ParseToDateTime(clanWarInfoFromApi.StartTime);

        var existingCurrentCw = trackedClanBuilder.Clan.ClanWars.
            FirstOrDefault(x => x.StartedOn == cwStartedOn);

        NotFoundException.ThrowByPredicate(() => existingCurrentCw == null, "UpdateCurrentClanWarToClan - is failed, no last clanWar found");

        var clanWarBuilder = new ClanWarBuilder(existingCurrentCw);

        clanWarBuilder.SetBaseProperties(clanWarInfoFromApi);

        clanWarBuilder.SetOpponentWarStatistics(ClanInfoRequest.CallApi(clanTag).Result);

        clanWarBuilder.SetTrackedClan(trackedClanBuilder.Clan);

        clanWarBuilder = UpdateEnemyWarMembers(clanWarBuilder, clanWarInfoFromApi);

        clanWarBuilder = UpdateCwMembersWithAttacks(trackedClanBuilder, clanWarBuilder, clanWarInfoFromApi);

        dbContext.SaveChanges();
    }

    public static void UpdateCurrentCwlClanWars(string clanTag)
    {
        var cwlGroupRequest = CwlGroupRequest.CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => cwlGroupRequest == null, "UpdateCwlClanWars -  is failed, clan is not in CWL group");

        var cwlWarsApi = AddToDbCommandHandler.GetCwlWars(cwlGroupRequest, clanTag);

        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null || trackedClanDb.Id == 0, "UpdateCwlClanWars - is failed, no such clan found");

        foreach (var cwlWarInfoFromApi in cwlWarsApi)
        {
            var cwlCwStartedOn = DateTimeParser.ParseToDateTime(cwlWarInfoFromApi.StartTime);

            var trackedClanBuilder = new TrackedClanBuilder(trackedClanDb);

            var existingCurrentCw = trackedClanBuilder.Clan.ClanWars.FirstOrDefault(x => x.StartedOn == cwlCwStartedOn);

            if (existingCurrentCw == null || existingCurrentCw.State == "warEnded")
            {
                continue;
            }

            var clanWarBuilder = new ClanWarBuilder(existingCurrentCw);

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

            clanWarBuilder = UpdateEnemyWarMembers(clanWarBuilder, cwlWarInfoFromApi);

            clanWarBuilder = UpdateCwMembersWithAttacks(trackedClanBuilder, clanWarBuilder, cwlWarInfoFromApi);
        }

        dbContext.SaveChanges();
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
                    var warAttackBuilder = new WarAttackBuilder(existingWarMember?.WarAttacks?
                        .FirstOrDefault(x => x.AttackOrder == warAttack.Order));

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

            clanMemberOnWar?.WarMemberships.Add(warMemberBuilder.WarMember);

            warMembers.Add(warMemberBuilder.WarMember);
        }

        clanWarBuilder.SetWarMembers(warMembers);

        return clanWarBuilder;
    }


    public static void ResetClanAdminKey(string clanTag, string newAdminsKey)
    {
        using AppDbContext dbContext = new();

        dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag).AdminsKey = newAdminsKey;

        dbContext.SaveChanges();
    }

    public static void ResetClanIsBlasckListProperty(string clanTag, bool isInBlackList)
    {
        using AppDbContext dbContext = new();

        dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag).IsInBlackList = isInBlackList;

        dbContext.SaveChanges();
    }

    public static void ResetClanChatId(string clanTag, string newChatId)
    {
        using AppDbContext dbContext = new();

        dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag).ClansTelegramChatId = newChatId;

        dbContext.SaveChanges();
    }

    public static void ResetClanRegularNewsLetter(string clanTag, NewsLetterType newsLetterType, int customTime = 0)
    {
        using AppDbContext dbContext = new();

        var clan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        switch (newsLetterType)
        {
            case NewsLetterType.All:
                {
                    clan.RegularNewsLetterOn = !clan.RegularNewsLetterOn;

                    break;
                }
            case NewsLetterType.WarStart:
                {
                    clan.WarStartMessageOn = !clan.WarStartMessageOn;

                    break;
                }
            case NewsLetterType.WarEnd:
                {
                    clan.WarEndMessageOn = !clan.WarEndMessageOn;

                    break;
                }
            case NewsLetterType.WarCustomTime:
                {
                    clan.WarTimeToMessageBeforeEnd = customTime;

                    break;
                }
            case NewsLetterType.RaidStart:
                {
                    clan.RaidStartMessageOn = !clan.RaidStartMessageOn;

                    break;
                }
            case NewsLetterType.RaidEnd:
                {
                    clan.RaidEndMessageOn = !clan.RaidEndMessageOn;

                    break;
                }
            case NewsLetterType.RaidCustomTime:
                {
                    clan.RaidTimeToMessageBeforeEnd = customTime;

                    break;
                }
            default:
                {
                    throw new NotFoundException("ResetClanRegularNewsLetter Не смог определить тип  NewsLetterType");
                }
        }

        dbContext.SaveChanges();
    }

    public static void ResetMemberUserName(string memberTag, string newUserName)
    {
        using AppDbContext dbContext = new();

        dbContext.ClanMembers.FirstOrDefault(x => x.Tag == memberTag).TelegramUserName = newUserName;

        dbContext.SaveChanges();
    }
}

public enum NewsLetterType
{
    All,

    WarStart,
    WarEnd,
    WarCustomTime,

    RaidStart,
    RaidEnd,
    RaidCustomTime
}