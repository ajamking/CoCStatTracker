using CoCStatsTracker.UIEntities;
using CoCStatTracker;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoCStatsTracker;

public class GetFromDbQueryHandler
{
    private string _dbConnectionString;

    public GetFromDbQueryHandler(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
    }

    public List<ClanUi> GetAllTrackedClans()
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiClans = new List<ClanUi>();

            uiClans.AddRange(dbContext.TrackedClans
                .Select(Mapper.MapToUi).ToList());

            NotFoundException.ThrowByPredicate(() => uiClans is { Count: 0 }, "No tracked clans were found in DB");

            return uiClans;
        }
    }

    public List<CwCwlUi> GetAllClanWars(string clanTag)
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

    public List<RaidsUi> GetAllRaids(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiRaids = new List<RaidsUi>();

            uiRaids.AddRange(dbContext.CapitalRaids
                .Where(x => x.TrackedClan.Tag == clanTag)
                .Select(Mapper.MapToUi).ToList());

            NotFoundException.ThrowByPredicate(() => uiRaids is { Count: 0 }, "No tracked Raids were found in DB");

            return uiRaids;
        }
    }

    public List<ClanMemberUi> GetAllClanMembers(string clanTag)
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

    public ArmyUi GetAllMemberUnits(string playersTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiArmy = Mapper.MapToUi(dbContext.ClanMembers
               .FirstOrDefault(x => x.Tag == playersTag)?.Units);

            NotFoundException.ThrowByPredicate(() => uiArmy is { Units.Count: 0 }, "No tracked Units were found in DB");

            return uiArmy;
        }
    }

    public List<CwCwlMembershipUi> GetAllMemberСwCwlMemberships(string playersTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiCwCwlMemberships = new List<CwCwlMembershipUi>();

            uiCwCwlMemberships.AddRange(dbContext.ClanMembers
                .FirstOrDefault(x => x.Tag == playersTag)?.WarMemberships?
                .Select(Mapper.MapToUi).ToList());

            NotFoundException.ThrowByPredicate(() => uiCwCwlMemberships is { Count: 0 }, "No tracked CwCwlMemberships were found in DB");

            return uiCwCwlMemberships;
        }
    }

    public List<RaidMembershipUi> GetAllMemberRaidMemberships(string playersTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var uiRaidMemberships = new List<RaidMembershipUi>();

            uiRaidMemberships.AddRange(dbContext.ClanMembers
                .FirstOrDefault(x => x.Tag == playersTag).RaidMemberships
                .Select(Mapper.MapToUi).ToList());

            NotFoundException.ThrowByPredicate(() => uiRaidMemberships is { Count: 0 }, "No tracked RaidMemberships were found in DB");

            return uiRaidMemberships;
        }
    }

    public AverageRaidsPerfomanceUi GetAverageRaidsPerfomance(string playersTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var raidMemberships = new List<RaidMember>();

            raidMemberships.AddRange(dbContext.ClanMembers
                .FirstOrDefault(x => x.Tag == playersTag).RaidMemberships.ToList());

            NotFoundException.ThrowByPredicate(() => raidMemberships is { Count: 0 }, "No tracked RaidMemberships were found in DB");

            return Mapper.MapToUi(raidMemberships);
        }
    }

}