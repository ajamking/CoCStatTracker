using CoCStatsTracker.UIEntities;
using Storage;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CoCStatsTracker;

public static class GetFromDbQueryHandler
{
    private static string _dbConnectionString = "Data Source=./../../../../CustomSolutionElements/CoCStatsTracker.db";

    public static void SetConnectionString(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
    }

    /*--------------Клан--------------*/
    public static List<ClanUi> GetAllTrackedClans()
    {
        var test = new Stopwatch();

        test.Start();

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
           
            var uiClans = new List<ClanUi>();

            uiClans.AddRange(dbContext.TrackedClans
                .Select(Mapper.MapToUi).ToList());

            NotFoundException.ThrowByPredicate(() => uiClans is { Count: 0 }, "No tracked clans were found in DB");

            test.Stop();

            return uiClans;
        }
    }

    public static ClanUi GetTrackedClan(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiClan = Mapper.MapToUi(dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag));

            NotFoundException.ThrowByPredicate(() => uiClan is null, "No such Clan was found in DB");

            return uiClan;
        }
    }

    public static List<CwCwlUi> GetAllClanWars(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiClanWars = new List<CwCwlUi>();

            uiClanWars.AddRange(dbContext.ClanWars
                .Where(x => x.TrackedClan.Tag == clanTag)
                .Select(Mapper.MapToUi).ToList());

            NotFoundException.ThrowByPredicate(() => uiClanWars is { Count: 0 }, "No tracked ClanWars were found in DB");

            return uiClanWars;
        }
    }

    public static List<RaidUi> GetAllRaids(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiRaids = new List<RaidUi>();

            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan is null, "No such clan was found in DB");

            foreach (var raid in trackedClan.CapitalRaids)
            {
                uiRaids.Add(Mapper.MapToUi(raid, trackedClan));
            }

            NotFoundException.ThrowByPredicate(() => uiRaids is { Count: 0 }, "No tracked Raids were found in DB");

            return uiRaids;
        }
    }

    public static List<ClanMemberUi> GetAllClanMembers(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiClanMembers = new List<ClanMemberUi>();

            uiClanMembers.AddRange(dbContext.ClanMembers
                .Where(x => x.Clan.Tag == clanTag)
                .Select(Mapper.MapToUi).ToList());

            NotFoundException.ThrowByPredicate(() => uiClanMembers is { Count: 0 }, "No tracked ClanMembers were found in DB");

            return uiClanMembers;
        }
    }

    public static List<AverageRaidsPerfomanceUi> GetAllClanMembersAverageRaidPerfomance(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var clan = dbContext.TrackedClans
                .FirstOrDefault(x => x.Tag == clanTag);

            var clanMembers = clan.ClanMembers.ToList();

            NotFoundException.ThrowByPredicate(() => clanMembers is { Count: 0 }, "No ClanMembers was found in DB");

            var averagePerfomances = new List<AverageRaidsPerfomanceUi>();

            foreach (var member in clan.ClanMembers)
            {
                if (member.RaidMemberships != null && member.RaidMemberships.Count != 0)
                {
                    averagePerfomances.Add(Mapper.MapToUi(member.RaidMemberships, member.Clan));
                }
            }

            NotFoundException.ThrowByPredicate(() => averagePerfomances is { Count: 0 }, "No tracked RaidMemberships were found in DB");

            return averagePerfomances;
        }
    }

    public static List<SeasonStatisticsUi> GetSeasonStatistics(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var clan = dbContext.TrackedClans.First(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => clan is null, "No such Clan was found in DB");

            var currentClanMembers = clan.ClanMembers;

            var obsoleteClanMembers = clan.LastClanMembersStaticstics.ObsoleteClanMembers;

            var seasonStatistics = new List<SeasonStatisticsUi>();

            foreach (var member in currentClanMembers)
            {
                if (obsoleteClanMembers.Any(x => x.Tag == member.Tag))
                {
                    seasonStatistics.Add(Mapper.MapToUi(member, obsoleteClanMembers.First(x => x.Tag == member.Tag)));
                }
            }

            NotFoundException.ThrowByPredicate(() => seasonStatistics is { Count: 0 }, "Can`t calculate seasonalStatistics");

            return seasonStatistics;
        }
    }


    /*--------------Клан мембер--------------*/
    public static ClanMemberUi GetClanMember(string playersTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiClanMember = Mapper.MapToUi(dbContext.ClanMembers.FirstOrDefault(x => x.Tag == playersTag));

            NotFoundException.ThrowByPredicate(() => uiClanMember is null, "No such ClanMember was found in DB");

            return uiClanMember;
        }
    }

    public static ArmyUi GetMembersArmy(string playersTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var searchingClanMember = dbContext.ClanMembers
               .FirstOrDefault(x => x.Tag == playersTag);

            var uiArmy = Mapper.MapToUi(searchingClanMember?.Units, searchingClanMember);

            NotFoundException.ThrowByPredicate(() => uiArmy is { Units.Count: 0 }, "No tracked Units were found in DB");

            return uiArmy;
        }
    }

    public static List<CwCwlMembershipUi> GetAllMemberСwCwlMemberships(string playersTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiCwCwlMemberships = new List<CwCwlMembershipUi>();

            uiCwCwlMemberships.AddRange(dbContext.ClanMembers
                .FirstOrDefault(x => x.Tag == playersTag)?.WarMemberships?
                .Select(Mapper.MapToUi).ToList());

            NotFoundException.ThrowByPredicate(() => uiCwCwlMemberships is { Count: 0 }, "No tracked CwCwlMemberships were found in DB");

            uiCwCwlMemberships.OrderByDescending(x => x.StartedOn);

            return uiCwCwlMemberships;
        }
    }

    public static List<RaidMembershipUi> GetAllMemberRaidMemberships(string playersTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiRaidMemberships = new List<RaidMembershipUi>();

            uiRaidMemberships.AddRange(dbContext.ClanMembers
                .FirstOrDefault(x => x.Tag == playersTag).RaidMemberships
                .Select(Mapper.MapToUi).ToList());

            NotFoundException.ThrowByPredicate(() => uiRaidMemberships is { Count: 0 }, "No tracked RaidMemberships were found in DB");

            uiRaidMemberships.OrderByDescending(x => x.StartedOn);

            return uiRaidMemberships;
        }
    }


}