using CoCStatsTracker.UIEntities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Storage;
using System.Collections.Generic;
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
    public static List<ClanUi> GetAllTrackedClansUi()
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiClans = new List<ClanUi>();

            uiClans.AddRange(dbContext.TrackedClans
                .Select(Mapper.MapToUi).ToList());

            NotFoundException.ThrowByPredicate(() => uiClans.Count == 0, "No tracked clans were found in DB");

            return uiClans;
        }
    }

    public static ClanUi GetTrackedClanUi(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiClan = Mapper.MapToUi(dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag));

            NotFoundException.ThrowByPredicate(() => uiClan == null, "No such Clan was found in DB");

            return uiClan;
        }
    }

    public static List<CwCwlUi> GetAllClanWarsUi(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiClanWars = new List<CwCwlUi>();

            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan == null, "No such clan was found in DB");

            var wars = trackedClan.ClanWars;

            NotFoundException.ThrowByPredicate(() => wars.Count == 0, "No tracked ClanWars were found in DB");

            foreach (var clanWar in wars)
            {
                uiClanWars.Add(Mapper.MapToUi(clanWar));
            }

            return uiClanWars;
        }
    }

    public static List<RaidUi> GetAllRaidsUi(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiRaids = new List<RaidUi>();

            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan == null, "No such clan was found in DB");

            var raids = trackedClan.CapitalRaids;

            NotFoundException.ThrowByPredicate(() => raids.Count == 0, "No tracked Raids were found in DB");

            foreach (var raid in trackedClan.CapitalRaids)
            {
                uiRaids.Add(Mapper.MapToUi(raid, trackedClan));
            }

            return uiRaids;
        }
    }

    public static List<ClanMemberUi> GetAllClanMembersUi(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiClanMembers = new List<ClanMemberUi>();

            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan == null, "No such clan was found in DB");

            var clanMembers = trackedClan.ClanMembers;

            NotFoundException.ThrowByPredicate(() => clanMembers.Count == 0, "No tracked ClanMembers were found in DB");

            foreach (var member in trackedClan.ClanMembers)
            {
                uiClanMembers.Add(Mapper.MapToUi(member));
            }

            return uiClanMembers;
        }
    }

    public static List<AverageRaidsPerfomanceUi> GetAllClanMembersAverageRaidPerfomanceUi(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var clan = dbContext.TrackedClans
                .FirstOrDefault(x => x.Tag == clanTag);

            var clanMembers = clan.ClanMembers.ToList();

            NotFoundException.ThrowByPredicate(() => clanMembers.Count == 0, "No ClanMembers was found in DB");

            var averagePerfomances = new List<AverageRaidsPerfomanceUi>();

            foreach (var member in clanMembers)
            {
                if (member.RaidMemberships != null && member.RaidMemberships.Count != 0)
                {
                    averagePerfomances.Add(Mapper.MapToUi(member.RaidMemberships, member.TrackedClan));
                }
            }

            NotFoundException.ThrowByPredicate(() => averagePerfomances.Count == 0, "No tracked RaidMemberships were found in DB");

            return averagePerfomances;
        }
    }

    public static List<SeasonStatisticsUi> GetSeasonStatisticsUi(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var clan = dbContext.TrackedClans.First(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => clan == null, "No such Clan was found in DB");

            var currentClanMembers = clan.ClanMembers;

            var obsoleteClanMembers = clan.PreviousClanMembersStaticstics;

            var seasonStatistics = new List<SeasonStatisticsUi>();

            foreach (var member in currentClanMembers)
            {
                if (obsoleteClanMembers.Any(x => x.Tag == member.Tag))
                {
                    seasonStatistics.Add(Mapper.MapToUi(member, obsoleteClanMembers.First(x => x.Tag == member.Tag), clan.PreviousClanMembersStaticstics.FirstOrDefault().UpdatedOn));
                }
            }

            NotFoundException.ThrowByPredicate(() => seasonStatistics.Count == 0, "Can`t calculate seasonalStatistics");

            return seasonStatistics;
        }
    }


    /*--------------Клан мембер--------------*/
    public static ClanMemberUi GetClanMemberUi(string playersTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiClanMember = Mapper.MapToUi(dbContext.ClanMembers.FirstOrDefault(x => x.Tag == playersTag));

            NotFoundException.ThrowByPredicate(() => uiClanMember == null, "No such ClanMember was found in DB");

            return uiClanMember;
        }
    }

    public static ArmyUi GetMembersArmyUi(string playersTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var searchingClanMember = dbContext.ClanMembers
               .FirstOrDefault(x => x.Tag == playersTag);

            var uiArmy = Mapper.MapToUi(searchingClanMember?.Units, searchingClanMember);

            NotFoundException.ThrowByPredicate(() => uiArmy.Units.Count == 0, "No tracked Units were found in DB");

            return uiArmy;
        }
    }

    public static List<CwCwlMembershipUi> GetAllMemberСwCwlMembershipsUi(string playersTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiCwCwlMemberships = new List<CwCwlMembershipUi>();

            uiCwCwlMemberships.AddRange(dbContext.ClanMembers
                .FirstOrDefault(x => x.Tag == playersTag)?.WarMemberships?
                .Select(Mapper.MapToUi).ToList());

            NotFoundException.ThrowByPredicate(() => uiCwCwlMemberships.Count == 0, "No tracked CwCwlMemberships were found in DB");

            uiCwCwlMemberships.OrderByDescending(x => x.StartedOn);

            return uiCwCwlMemberships;
        }
    }

    public static List<RaidMembershipUi> GetAllMemberRaidMembershipsUi(string playersTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiRaidMemberships = new List<RaidMembershipUi>();

            uiRaidMemberships.AddRange(dbContext.ClanMembers
                .FirstOrDefault(x => x.Tag == playersTag).RaidMemberships
                .Select(Mapper.MapToUi).ToList());

            NotFoundException.ThrowByPredicate(() => uiRaidMemberships.Count == 0, "No tracked RaidMemberships were found in DB");

            uiRaidMemberships.OrderByDescending(x => x.StartedOn);

            return uiRaidMemberships;
        }
    }


    /*--------------Для проверки вводимых тегов--------------*/
    public static bool CheckClanExists(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            return dbContext.TrackedClans
                .Where(x => x.IsInBlackList == false)
                .Any(x => x.Tag == clanTag);
        }
    }

    public static bool CheckMemberExists(string memberTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            return dbContext.ClanMembers
                .Where(x=>x.TrackedClan.IsInBlackList==false)
                .Any(x => x.Tag == memberTag);
        }
    }

    public static List<TrackedClan> GetAllTrackedClans()
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            return dbContext.TrackedClans.ToList();
        }
    }
}