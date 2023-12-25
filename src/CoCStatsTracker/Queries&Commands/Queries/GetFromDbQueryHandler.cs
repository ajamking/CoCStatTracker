using CoCStatsTracker.UIEntities;
using Domain.Entities;
using Storage;
using System.Collections.Generic;
using System.Linq;

namespace CoCStatsTracker;

public static class GetFromDbQueryHandler
{
    /*--------------Клан--------------*/
    public static List<TrackedClanUi> GetAllTrackedClansUi()
    {
        using AppDbContext dbContext = new();

        var trackedClansUi = new List<TrackedClanUi>();

        var trackedClansDb = dbContext.TrackedClans;

        NotFoundException.ThrowByPredicate(() => trackedClansDb == null || !trackedClansDb.Any(), "GetAllTrackedClansUi - No tracked clans were found in DB");

        trackedClansUi.AddRange(trackedClansDb.Select(Mapper.MapToUi).ToList());

        return trackedClansUi;
    }

    public static TrackedClanUi GetTrackedClanUi(string clanTag)
    {
        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null, "GetTrackedClanUi - No such Clan was found in DB");

        return Mapper.MapToUi(trackedClanDb);
    }

    public static List<TrackedClan> GetAllTrackedClans()
    {
        using AppDbContext dbContext = new();

        var trackedClansDb = dbContext.TrackedClans;

        NotFoundException.ThrowByPredicate(() => trackedClansDb == null || !trackedClansDb.Any(), "GetAllTrackedClans - No tracked clans were found in DB");

        return trackedClansDb.ToList();
    }

    public static TrackedClan GetTrackedClan(string clanTag)
    {
        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null, "GetTrackedClan - No such Clan was found in DB");

        return trackedClanDb;
    }


    public static List<ClanMemberUi> GetAllClanMembersUi(string clanTag)
    {
        using AppDbContext dbContext = new();

        var clanMembersUi = new List<ClanMemberUi>();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null, "GetAllClanMembersUi - No such clan was found in DB");

        NotFoundException.ThrowByPredicate(() => trackedClanDb.ClanMembers == null || trackedClanDb.ClanMembers.Count == 0, "GetAllClanMembersUi - No tracked ClanMembers were found in DB");

        var clanMembersDb = trackedClanDb.ClanMembers;

        foreach (var member in trackedClanDb.ClanMembers)
        {
            clanMembersUi.Add(Mapper.MapToUi(member));
        }

        return clanMembersUi;
    }

    public static List<ClanMember> GetAllClanMembers(string clanTag)
    {
        using AppDbContext dbContext = new();

        var clanMembers = new List<ClanMember>();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null, "GetAllClanMembers - No such clan was found in DB");

        NotFoundException.ThrowByPredicate(() => trackedClanDb.ClanMembers == null || trackedClanDb.ClanMembers.Count == 0, "GetAllClanMembers - No tracked ClanMembers were found in DB");

        return trackedClanDb.ClanMembers.ToList();
    }

    public static List<MedianRaidPerfomanseUi> GetAverageRaidmembersPerfomanceUi(string clanTag)
    {
        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null, "GetAverageRaidmembersPerfomanceUi - No such clan was found in DB");

        NotFoundException.ThrowByPredicate(() => trackedClanDb.ClanMembers == null || trackedClanDb.ClanMembers.Count == 0, "GetAverageRaidmembersPerfomanceUi - No tracked ClanMembers were found in DB");

        var clanMembersDb = trackedClanDb.ClanMembers;

        var averagePerfomancesUi = new List<MedianRaidPerfomanseUi>();

        foreach (var member in clanMembersDb)
        {
            if (member.RaidMemberships != null && member.RaidMemberships.Count != 0)
            {
                averagePerfomancesUi.Add(Mapper.MapToUi(member.RaidMemberships, member.TrackedClan));
            }
        }

        NotFoundException.ThrowByPredicate(() => averagePerfomancesUi.Count == 0, "GetAverageRaidmembersPerfomanceUi - No tracked RaidMemberships were found in DB");

        return averagePerfomancesUi;
    }

    public static List<SeasonStatisticsUi> GetSeasonStatisticsUi(string clanTag)
    {
        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.First(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null, "GetSeasonStatisticsUi - No such Clan was found in DB");

        NotFoundException.ThrowByPredicate(() => trackedClanDb.ClanMembers == null || trackedClanDb.ClanMembers.Count == 0, "GetAverageRaidmembersPerfomanceUi - No tracked ClanMembers were found in DB");

        var currentClanMembersDb = trackedClanDb.ClanMembers;

        var obsoleteClanMembersDb = trackedClanDb.PreviousClanMembersStaticstics;

        var seasonStatisticsUi = new List<SeasonStatisticsUi>();

        foreach (var member in currentClanMembersDb)
        {
            if (obsoleteClanMembersDb.Any(x => x.Tag == member.Tag))
            {
                seasonStatisticsUi.Add(Mapper.MapToUi(member, obsoleteClanMembersDb.First(x => x.Tag == member.Tag), trackedClanDb.PreviousClanMembersStaticstics.FirstOrDefault().UpdatedOn));
            }
        }

        NotFoundException.ThrowByPredicate(() => seasonStatisticsUi.Count == 0, "GetAverageRaidmembersPerfomanceUi - Can`t calculate seasonalStatistics");

        return seasonStatisticsUi;
    }

    public static List<PreviousClanMember> GetClanPreviousClanMembers(string clanTag)
    {
        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.First(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null, "GetClanPreviousClanMembers - No such Clan was found in DB");

        return trackedClanDb.PreviousClanMembersStaticstics.ToList();
    }

    /*--------------Клан Войны--------------*/
    public static List<ClanWarUi> GetAllClanWarsUi(string clanTag)
    {
        using AppDbContext dbContext = new();

        var clanWarsUi = new List<ClanWarUi>();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null, "GetAllClanWarsUi - No such clan was found in DB");

        var clanWarsDb = trackedClanDb.ClanWars;

        NotFoundException.ThrowByPredicate(() => clanWarsDb == null || clanWarsDb.Count == 0, "GetAllClanWarsUi - No tracked ClanWars were found in DB");

        clanWarsUi.AddRange(clanWarsDb.Select(Mapper.MapToUi).ToList());

        return clanWarsUi;
    }

    public static ClanWarUi GetLastClanWarUi(string clanTag)
    {
        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null, "GetLastClanWarUi - No such clan was found in DB");

        NotFoundException.ThrowByPredicate(() => trackedClanDb.ClanWars == null || trackedClanDb.ClanWars.Count == 0, "GetLastClanWarUi - No tracked ClanWars were found in DB");

        var lastWarDb = trackedClanDb.ClanWars.OrderByDescending(x => x.EndedOn).First();

        return Mapper.MapToUi(lastWarDb);
    }


    /*--------------Клан Рейды--------------*/
    public static List<CapitalRaidUi> GetAllRaidsUi(string clanTag)
    {
        using AppDbContext dbContext = new();

        var capitalRaidsUi = new List<CapitalRaidUi>();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null, "GetAllRaidsUi - No such clan was found in DB");

        NotFoundException.ThrowByPredicate(() => trackedClanDb.CapitalRaids == null || trackedClanDb.CapitalRaids.Count == 0, "GetAllRaidsUi - No tracked Raids were found in DB");

        var capitalRaidsDb = trackedClanDb.CapitalRaids;

        foreach (var raid in trackedClanDb.CapitalRaids)
        {
            capitalRaidsUi.Add(Mapper.MapToUi(raid, trackedClanDb));
        }

        return capitalRaidsUi;
    }

    public static CapitalRaidUi GetLastRaidUi(string clanTag)
    {
        using AppDbContext dbContext = new();

        var trackedClanDb = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        NotFoundException.ThrowByPredicate(() => trackedClanDb == null, "GetLastRaidUi - No such clan was found in DB");

        NotFoundException.ThrowByPredicate(() => trackedClanDb.CapitalRaids == null || trackedClanDb.CapitalRaids.Count == 0, "GetLastRaidUi - No tracked Raids were found in DB");

        var lastRaidDb = trackedClanDb.CapitalRaids.OrderByDescending(x => x.EndedOn).First();

        return Mapper.MapToUi(lastRaidDb, trackedClanDb);
    }


    /*--------------Клан мембер--------------*/
    public static ClanMemberUi GetClanMemberUi(string playersTag)
    {
        using AppDbContext dbContext = new();

        var clanMemberDb = dbContext.ClanMembers.FirstOrDefault(x => x.Tag == playersTag);

        NotFoundException.ThrowByPredicate(() => clanMemberDb == null, "GetClanMemberUi - No such ClanMember was found in DB");

        var uiClanMember = Mapper.MapToUi(clanMemberDb);

        return uiClanMember;
    }

    public static ArmyUi GetMembersArmyUi(string playersTag)
    {
        using AppDbContext dbContext = new();

        var clanMemberDb = dbContext.ClanMembers.FirstOrDefault(x => x.Tag == playersTag);

        NotFoundException.ThrowByPredicate(() => clanMemberDb == null, "GetMembersArmyUi - No such ClanMember was found in DB");

        NotFoundException.ThrowByPredicate(() => clanMemberDb.Units == null || clanMemberDb.Units.Count == 0, "GetMembersArmyUi - No tracked Units were found in DB");

        var armyUi = Mapper.MapToUi(clanMemberDb?.Units, clanMemberDb);

        return armyUi;
    }

    public static List<WarMembershipsUi> GetAllWarMembershipsUi(string playersTag)
    {
        using AppDbContext dbContext = new();

        var warMembershipsDb = dbContext.ClanMembers.FirstOrDefault(x => x.Tag == playersTag).WarMemberships;

        NotFoundException.ThrowByPredicate(() => warMembershipsDb == null || warMembershipsDb.Count == 0, "GetAllWarMembershipsUi - No tracked WarMemberships were found in DB");

        var uiCwCwlMemberships = new List<WarMembershipsUi>();

        uiCwCwlMemberships.AddRange(warMembershipsDb.Select(Mapper.MapToUi).ToList());

        return uiCwCwlMemberships;
    }

    public static List<RaidMembershipUi> GetAllMemberRaidMembershipsUi(string playersTag)
    {
        using AppDbContext dbContext = new();

        var raidMembershipsDb = dbContext.ClanMembers.FirstOrDefault(x => x.Tag == playersTag).RaidMemberships;

        NotFoundException.ThrowByPredicate(() => raidMembershipsDb == null || raidMembershipsDb.Count == 0, "GetAllMemberRaidMembershipsUi - No tracked WarMemberships were found in DB");

        var uiRaidMemberships = new List<RaidMembershipUi>();

        uiRaidMemberships.AddRange(raidMembershipsDb.Select(Mapper.MapToUi).ToList());

        return uiRaidMemberships;
    }

    /*--------------Для проверки вводимых тегов--------------*/
    public static bool CheckClanExists(string clanTag)
    {
        using AppDbContext dbContext = new();

        return dbContext.TrackedClans
            .Where(x => x.IsInBlackList == false)
            .Any(x => x.Tag == clanTag);
    }

    public static bool CheckMemberExists(string memberTag)
    {
        using AppDbContext dbContext = new();

        return dbContext.ClanMembers
            .Where(x => x.TrackedClan.IsInBlackList == false)
            .Any(x => x.Tag == memberTag);
    }
}