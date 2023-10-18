using CoCApiDealer.ApiRequests;
using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Builders;
using CoCStatsTracker.Helpers;
using CoCStatsTracker.Items.Exceptions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;

namespace CoCStatsTracker.Items.Queries_Commands.Commands;

public class UpdateDbCommandHandler
{
    private string _dbConnectionString;

    public UpdateDbCommandHandler(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
    }

    public void UpdateTrackedClanBaseProperties(string clanTag)
    {
        var clanInfoFromApi = new ClanInfoRequest().CallApi(clanTag).Result;

        FailedPullFromApiException.ThrowByPredicate(() => clanInfoFromApi is { Tag: null }, "UpdateTrackedClanBaseProperties is failed, bad API responce");

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan is { Id: 0 }, "UpdateTrackedClanBaseProperties is failed, no such clan found");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            trackedClanBuilder.SetBaseProperties(clanInfoFromApi);

            dbContext.SaveChanges();
        }
    }

    public void UpdateTrackedClanClanMembers(string clanTag)
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


    public void UpdateClanCurrentRaid(string clanTag)
    {
        var raidInfoFromApi = new CapitalRaidsRequest().CallApi(clanTag, 1).Result.RaidsInfo.First();

        FailedPullFromApiException.ThrowByPredicate(() => raidInfoFromApi is { StartTime: null }, "AddCurrentRaidToClan is failed, bad API responce");

        using (AppDbContext dbContext = new AppDbContext(_dbConnectionString))
        {
            var trackedClan = dbContext.TrackedClans.FirstOrDefault(x => x.Tag == clanTag);

            NotFoundException.ThrowByPredicate(() => trackedClan is { Id: 0 }, "AddCurrentRaidToClan is failed, no such clan found");

            var trackedClanBuilder = new TrackedClanBuilder(trackedClan);

            var raidStartedOn = DateTimeParser.Parse(raidInfoFromApi.StartTime);

            var existingCurrentRaid = trackedClanBuilder.Clan.CapitalRaids
          .FirstOrDefault(x => x.StartedOn == raidStartedOn);

            var raidBuilder = new CapitalRaidBuilder(existingCurrentRaid);

            raidBuilder.SetBaseProperties(raidInfoFromApi);

            raidBuilder.SetTrackedClan(trackedClanBuilder.Clan);

            raidBuilder = UpdateRadeDefences(raidBuilder, raidInfoFromApi);

            raidBuilder = UpdateRaidMembersWithoutAttacks(trackedClanBuilder, raidBuilder, raidInfoFromApi);

            raidBuilder = UpdateDefeatedClansAndRaidAttacks(raidBuilder, raidInfoFromApi);

            dbContext.SaveChanges();
        }

    }

    private CapitalRaidBuilder UpdateRadeDefences(CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var existingRaidDefences = raidBuilder.Raid.RaidDefenses;

        var raidDefenseBuilder = new RaidDefenseBuilder(existingRaidDefences);

        raidDefenseBuilder.SetBaseProperties(raidInfoFromApi.RaidDefenses);

        raidDefenseBuilder.SetRaid(raidBuilder.Raid);

        raidBuilder.SetRaidDefenses(raidDefenseBuilder.Defenses);

        return raidBuilder;
    }

    private CapitalRaidBuilder UpdateRaidMembersWithoutAttacks(TrackedClanBuilder trackedClanBuilder, CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var newRaidMembers = new List<RaidMember>();

        foreach (var raidMemberApi in raidInfoFromApi.RaidMembers)
        {
            var existingRaidMember = raidBuilder.Raid.RaidMembers
              .FirstOrDefault(x => x.Tag == raidMemberApi.Tag);

            var raidMemberBuilder = new RaidMemberBuilder(existingRaidMember);

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

    private CapitalRaidBuilder UpdateDefeatedClansAndRaidAttacks(CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var defeatedClans = new List<DefeatedClan>();

        var raidAttacks = new List<RaidAttack>();

        foreach (var attackedClanApi in raidInfoFromApi.RaidOnClans)
        {
            var existingDefeatedClan = raidBuilder.Raid.DefeatedClans
               .FirstOrDefault(x => x.DefendersTag == attackedClanApi.DefenderClan.Tag);

            var defeatedClanBuilder = new DefeatedClanBuilder(existingDefeatedClan);

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


    public void UpdateCurrentClanWarToClan(string clanTag, bool isCwLWar = false, string cwlWarTag = "")
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

    private ClanWarBuilder UpdateEnemyWarMembers(ClanWarBuilder clanWarBuilder, ClanWarApi clanWarInfoFromApi)
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

    private ClanWarBuilder UpdateCwMembersWithAttacks(TrackedClanBuilder trackedClanBuilder, ClanWarBuilder clanWarBuilder, ClanWarApi clanWarInfoFromApi)
    {
        var warMembers = new List<WarMember>();

        foreach (var warMemberApi in clanWarInfoFromApi.ClanResults.WarMembers)
        {
            var existingWarMember = clanWarBuilder.ClanWar.WarMembers
                .FirstOrDefault(x => x.Tag == warMemberApi.Tag);

            var warMemberBuilder = new WarMemberBuilder(existingWarMember);

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
