using Domain.Entities;
using Storage;
using System.Collections.Generic;
using System.Linq;

namespace CoCStatsTracker;

public static class DeleteFromDbCommandHandler
{
    private static string _dbConnectionString = "Data Source=./../../../../CustomSolutionElements/CoCStatsTracker.db";

    public static void SetConnectionString(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
    }

    public static void DeleteTrackedClan(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var clan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => clan == null, "DeleteTrackedClan is failed, no such clan found");

            dbContext.TrackedClans.Remove(clan);

            dbContext.SaveChanges();
        }
    }

    public static void DeleteClanWars(string clanTag, int countToSave)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var clanWars = dbContext.ClanWars
                .Where(x => x.TrackedClan.Tag == clanTag)
                .OrderByDescending(x => x.StartedOn)
                .ToList();

            NotFoundException.ThrowByPredicate(() => clanWars.Count == 0, "DeleteOldClanClanWars is failed, no tracked CWs for this clan");

            var clanWarsToRemove = new List<ClanWar>();

            for (int i = countToSave; i < clanWars.Count; i++)
            {
                dbContext.ClanWars.Remove(clanWars[i]);
            }

            dbContext.SaveChanges();
        }
    }

    public static void DeleteClanRaids(string clanTag, int countToSave)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var raids = dbContext.CapitalRaids
                .Where(x => x.TrackedClan.Tag == clanTag)
                .OrderByDescending(x => x.StartedOn)
                .ToList();

            NotFoundException.ThrowByPredicate(() => raids.Count == 0, "DeleteClanRaids is failed, no tracked Raids for this clan");

            var clanWarsToRemove = new List<ClanWar>();

            for (int i = countToSave; i < raids.Count; i++)
            {
                dbContext.CapitalRaids.Remove(raids[i]);
            }

            dbContext.SaveChanges();
        }
    }
}