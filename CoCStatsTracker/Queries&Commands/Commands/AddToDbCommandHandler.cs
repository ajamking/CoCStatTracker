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

public class AddToDbCommandHandler
{
    private string _dbConnectionString;

    public AddToDbCommandHandler(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
    }

    public void ResetDb()
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString, true))
        {
            dbContext.SaveChanges();
        }
    }

    public void AddTrackedClan(string clanTag)
    {
        var trackedClanBuilder = new TrackedClanBuilder();

        var clanInfoFromApi = new ClanInfoRequest().CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => clanInfoFromApi is { Tag: null }, "AddTrackedClan is failed, bad API responce");

        trackedClanBuilder.SetBaseProperties(clanInfoFromApi);

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            dbContext.TrackedClans.Add(trackedClanBuilder.Clan);

            dbContext.SaveChanges();
        }
    }

    public void AddClanMembers(string clanTag)
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
    }

    public void AddLastClanMembersStaticstics(string clanTag)
    {
        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            FailedPullFromApiException.ThrowByPredicate(() => trackedClan is { Id: 0 }, "AddLastClanMembersStaticstics is failed, no such clan found");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            trackedClanBuilder.SetLastClanMembersStaticstics(trackedClanBuilder.Clan.ClanMembers);
        }
    }

    public void AddCurrentRaidToClan(string clanTag)
    {
        var raidInfoFromApi = new CapitalRaidsRequest().CallApi(clanTag, 1).Result.RaidsInfo.First();

        FailedPullFromApiException.ThrowByPredicate(() => raidInfoFromApi is { StartTime: null }, "AddCurrentRaidToClan is failed, bad API responce");

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan is { Id: 0 }, "AddCurrentRaidToClan is failed, no such clan found");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            var raidBuilder = new CapitalRaidBuilder();

            raidBuilder.SetBaseProperties(raidInfoFromApi);

            raidBuilder.SetTrackedClan(trackedClanBuilder.Clan);

            raidBuilder = AddRaidDefenses(raidBuilder, raidInfoFromApi);

            raidBuilder = AddRaidMembersWithoutAttacks(trackedClanBuilder, raidBuilder, raidInfoFromApi);

            raidBuilder = AddDefeatedClansAndRaidAttacks(raidBuilder, raidInfoFromApi);

            trackedClanBuilder.AddCapitalRaid(raidBuilder.Raid);

            dbContext.SaveChanges();
        }
    }

    private CapitalRaidBuilder AddRaidDefenses(CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var raidDefenseBuilder = new RaidDefenseBuilder();

        raidDefenseBuilder.SetBaseProperties(raidInfoFromApi.RaidDefenses);

        raidDefenseBuilder.SetRaid(raidBuilder.Raid);

        raidBuilder.SetRaidDefenses(raidDefenseBuilder.Defenses);

        return raidBuilder;
    }

    private CapitalRaidBuilder AddRaidMembersWithoutAttacks(TrackedClanBuilder trackedClanBuilder, CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var newRaidMembers = new List<RaidMember>();

        foreach (var raidMemberApi in raidInfoFromApi.RaidMembers)
        {
            var raidMemberBuilder = new RaidMemberBuilder();

            raidMemberBuilder.SetBaseProperties(raidMemberApi);

            raidMemberBuilder.SetRaid(raidBuilder.Raid);

            var clanMemberOnRaid = trackedClanBuilder.Clan.ClanMembers
               .FirstOrDefault(x => x.Tag == raidMemberBuilder.Member.Tag);

            if (clanMemberOnRaid != null)
            {
                clanMemberOnRaid.RaidMemberships.Add(raidMemberBuilder.Member);
            }

            raidMemberBuilder.SetClanMember(clanMemberOnRaid);
        }

        raidBuilder.SetRaidMembers(newRaidMembers);

        return raidBuilder;
    }

    private CapitalRaidBuilder AddDefeatedClansAndRaidAttacks(CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var defeatedClans = new List<DefeatedClan>();

        var raidAttacks = new List<RaidAttack>();

        foreach (var attackedClanApi in raidInfoFromApi.RaidOnClans)
        {
            var defeatedClanBuilder = new DefeatedClanBuilder();

            defeatedClanBuilder.SetBaseProperties(attackedClanApi);

            defeatedClanBuilder.SetCapitalRaid(raidBuilder.Raid);

            var destroyedDistricts = new List<OpponentDistrict>();

            foreach (var defeatedDistrict in attackedClanApi.DestroyedDistricts)
            {
                var opponentDistrictBuilder = new OpponentDistrictBuilder();

                opponentDistrictBuilder.SetBaseProperties(defeatedDistrict);

                var sortedAttacks = new List<AttackOnDistrictApi>();

                if (defeatedDistrict.MemberAttacks != null)
                {
                    var previousDestructionPercent = 0;

                    sortedAttacks = defeatedDistrict.MemberAttacks.OrderBy(x => x.DestructionPercentTo).ToList();

                    foreach (var attack in sortedAttacks)
                    {
                        var tempRaidMember = raidBuilder.Raid.RaidMembers.FirstOrDefault(x => x.Tag == attack.Attacker.Tag);

                        var raidAttackBuilder = new RaidAttackBuilder(tempRaidMember.Attacks?
                            .FirstOrDefault(x => x.DestructionPercentTo == attack.DestructionPercentTo));

                        raidAttackBuilder.SetBaseProperties(previousDestructionPercent, attack, opponentDistrictBuilder.District);

                        raidAttackBuilder.SetRaidMember(raidBuilder.Raid.RaidMembers
                            .FirstOrDefault(x => x.Tag == attack.Attacker.Tag));

                        raidAttacks.Add(raidAttackBuilder.RaidAttack);

                        previousDestructionPercent = attack.DestructionPercentTo;
                    }
                }

                destroyedDistricts.Add(opponentDistrictBuilder.District);
            }

            defeatedClanBuilder.SetOpponentDistricts(destroyedDistricts);

            defeatedClans.Add(defeatedClanBuilder.Clan);
        }

        raidBuilder.SetDefeatedClans(defeatedClans);

        raidBuilder.SetAttacks(raidAttacks);

        return raidBuilder;
    }


    public void AddCurrentClanWarToClan(string clanTag, bool isCwLWar = false, string cwlWarTag = "")
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

            trackedClanBuilder.AddClanWar(clanWarBuilder.ClanWar);

            dbContext.SaveChanges();
        }
    }

    private ClanWarBuilder AddEnemyWarMembers(ClanWarBuilder clanWarBuilder, ClanWarApi clanWarInfoFromApi)
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

    private ClanWarBuilder AddCwMembersWithAttacks(TrackedClanBuilder trackedClanBuilder, ClanWarBuilder clanWarBuilder, ClanWarApi clanWarInfoFromApi)
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
