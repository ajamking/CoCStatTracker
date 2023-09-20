using CoCApiDealer.ApiRequests;
using CoCStatsTracker;
using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Builders;
using CoCStatsTracker.Helpers;
using CoCStatsTracker.UIEntities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CoCApiDealer;

public class DaddyBuilder
{
    public TrackedClanBuilder TrackedClanBuilder { get; }

    public DaddyBuilder(TrackedClan existingTrackedClan)
    {
        TrackedClanBuilder = new TrackedClanBuilder(existingTrackedClan);
    }

    //Заполняем базовую информацию о клане из API ответа
    public void UpdateClanBaseProperties()
    {
        var clanInfoFromApi = new ClanInfoRequest().CallApi(TrackedClanBuilder.Clan.Tag).Result;

        TrackedClanBuilder.SetBaseProperties(clanInfoFromApi);
    }

    //Поочередно заполняем базовую информацию и юнитов игроков из APi ответов
    public void UpdateClanMembersBasePropertiesAndUnits()
    {
        var clanMembersFromApi = new ClanInfoRequest().CallApi(TrackedClanBuilder.Clan.Tag).Result.Members;

        var updatedClanMembers = new List<ClanMember>();

        var SetMemberPropertyTasks = clanMembersFromApi.Select(async x =>
        {
            var playerInfoFromApi = await (new PlayerRequest().CallApi(x.Tag));

            var oldClanMember = TrackedClanBuilder.Clan.ClanMembers
                .FirstOrDefault(x => x.Tag == playerInfoFromApi.Tag);

            var clanMemberBuilder = new ClanMemberBuilder(oldClanMember);

            clanMemberBuilder.SetBaseProperties(playerInfoFromApi);

            clanMemberBuilder.SetUnits(playerInfoFromApi.Troops, playerInfoFromApi.Heroes);

            clanMemberBuilder.ClanMember.Clan = TrackedClanBuilder.Clan;

            updatedClanMembers.Add(clanMemberBuilder.ClanMember);
        }).ToList();

        Task.WhenAll(SetMemberPropertyTasks).GetAwaiter().GetResult();

        TrackedClanBuilder.SetClanMembers(updatedClanMembers);
    }

    /// <summary>
    /// Обновляем последний рейд если нашлось совпадение по времени начала, если нет - создаем новый.
    /// </summary>
    public void UpdateCurrentRaid()
    {
        var raidInfoFromApi = new CapitalRaidsRequest().CallApi(TrackedClanBuilder.Clan.Tag, 1).Result.RaidsInfo.First();

        var raidStartedOn = DateTimeParser.Parse(raidInfoFromApi.StartTime);

        var existingCurrentRaid = TrackedClanBuilder.Clan.CapitalRaids
            .FirstOrDefault(x => x.StartedOn == raidStartedOn);

        var raidBuilder = new CapitalRaidBuilder(existingCurrentRaid);

        raidBuilder.SetBaseProperties(raidInfoFromApi);

        raidBuilder.SetTrackedClan(TrackedClanBuilder.Clan);
      
        raidBuilder = AddRaidDefenses(raidBuilder, raidInfoFromApi);

        raidBuilder = AddRaidMembersWithoutAttacks(raidBuilder, raidInfoFromApi);

        raidBuilder = AddDefeatedClansAndRaidAttacks(raidBuilder, raidInfoFromApi);

        if (existingCurrentRaid == null)
        {
            TrackedClanBuilder.AddCapitalRaid(raidBuilder.Raid);
        }
        //else
        //{
        //    TrackedClanBuilder.Clan.CapitalRaids.Remove(existingCurrentRaid);

        //    TrackedClanBuilder.AddCapitalRaid(raidBuilder.Raid);
        //}
    }

    //Добавляем защиты с рейдов.
    private CapitalRaidBuilder AddRaidDefenses(CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var existingRaidDefences = raidBuilder.Raid.RaidDefenses;
            
        var raidDefenseBuilder = new RaidDefenseBuilder(existingRaidDefences);

        raidDefenseBuilder.SetBaseProperties(raidInfoFromApi.RaidDefenses);

        raidDefenseBuilder.SetRaid(raidBuilder.Raid);

        raidBuilder.SetRaidDefenses(raidDefenseBuilder.Defenses);

        return raidBuilder;
    }

    //Добавляем атаки участников рейдов и связываем все сущности включающие RaidMembers.
    private CapitalRaidBuilder AddRaidMembersWithoutAttacks(CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var newRaidMembers = new List<RaidMember>();

        foreach (var raidMemberApi in raidInfoFromApi.RaidMembers)
        {
            var existingRaidMember = raidBuilder.Raid.RaidMembers
               .FirstOrDefault(x => x.Tag == raidMemberApi.Tag);

            var raidMemberBuilder = new RaidMemberBuilder(existingRaidMember);

            raidMemberBuilder.SetBaseProperties(raidMemberApi);

            raidMemberBuilder.SetRaid(raidBuilder.Raid);

            var clanMemberOnRaid = TrackedClanBuilder.Clan.ClanMembers
               .FirstOrDefault(x => x.Tag == raidMemberBuilder.Member.Tag);

            if (clanMemberOnRaid != null)
            {
                clanMemberOnRaid.RaidMemberships.Add(raidMemberBuilder.Member);
            }

            raidMemberBuilder.SetClanMember(clanMemberOnRaid);

            var test = newRaidMembers;

            newRaidMembers.Add(raidMemberBuilder.Member);
        }

        raidBuilder.SetRaidMembers(newRaidMembers);

        return raidBuilder;
    }

    //Добавляем информацию о побежденных кланах и связываем атаки с атакуемым кланом.
    private CapitalRaidBuilder AddDefeatedClansAndRaidAttacks(CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
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

                var previousDestructionPercent = 0;

                if (defeatedDistrict.MemberAttacks != null)
                {
                    sortedAttacks = defeatedDistrict.MemberAttacks.OrderBy(x => x.DestructionPercentTo).ToList();

                    foreach (var attack in sortedAttacks)
                    {
                        var tempRaidMember = raidBuilder.Raid.RaidMembers.FirstOrDefault(x => x.Tag == attack.Attacker.Tag);

                        var raidAttackBuilder = new RaidAttackBuilder(tempRaidMember.Attacks?
                            .FirstOrDefault(x=>x.DestructionPercentTo == attack.DestructionPercentTo));

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





    /// <summary>
    /// Обновляем последний КВ если нашлось совпадение по времени начала, если нет - создаем новый.
    /// </summary>
    /// <param name="isCwLWar"> Флаг для определения типа войны </param>
    /// <param name="cwlWarTag"> Тег войны ЛВК. Нужно вводить если война все-таки в рамках ЛВК</param>
    public void UpdateCurrentClanWar(bool isCwLWar = false, string cwlWarTag = "")
    {
        //Определяем тип войны
        var clanWarInfoFromApi = isCwLWar ?
            new CwlWarRequest().CallApi(cwlWarTag).Result :
            new CurrentWarRequest().CallApi(TrackedClanBuilder.Clan.Tag).Result;

        var cwStartedOn = DateTimeParser.Parse(clanWarInfoFromApi.StartTime);

        var existingCurrentCw = TrackedClanBuilder.Clan.ClanWars.
            FirstOrDefault(x => x.StartedOn == cwStartedOn);

        var clanWarBuilder = new ClanWarBuilder(existingCurrentCw);

        clanWarBuilder.SetBaseProperties(clanWarInfoFromApi);

        clanWarBuilder.SetTrackedClan(TrackedClanBuilder.Clan);

        clanWarBuilder = AddEnemyWarMembers(clanWarBuilder, clanWarInfoFromApi);

        clanWarBuilder = AddCwMembersWithAttacks(clanWarBuilder, clanWarInfoFromApi);

        if (existingCurrentCw == null)
        {
            TrackedClanBuilder.AddClanWar(clanWarBuilder.ClanWar);
        }
        else
        {
            TrackedClanBuilder.Clan.ClanWars.Remove(existingCurrentCw);
            TrackedClanBuilder.AddClanWar(clanWarBuilder.ClanWar);
        }
    }

    //Добавляем список противников на КВ.
    private ClanWarBuilder AddEnemyWarMembers(ClanWarBuilder clanWarBuilder, ClanWarApi clanWarInfoFromApi)
    {
        var enemyWarmembers = new List<EnemyWarMember>();

        foreach (var enemyWarmember in clanWarInfoFromApi.OpponentResults.WarMembers)
        {
            var existingEnemuWarMember = clanWarBuilder.ClanWar.EnemyWarMembers
                .FirstOrDefault(x => x.Tag == enemyWarmember.Tag);

            var enemyWarMemberBuilder = new EnemyWarMemberBuilder(existingEnemuWarMember);

            enemyWarMemberBuilder.SetBaseProperties(enemyWarmember);

            enemyWarMemberBuilder.SetClanWar(clanWarBuilder.ClanWar);

            enemyWarmembers.Add(enemyWarMemberBuilder.EnemyWarMember);
        }

        clanWarBuilder.SetEnemyWarMembers(enemyWarmembers);

        return clanWarBuilder;
    }

    //Создаем и заполняем участников войны вместе с их атаками.
    private ClanWarBuilder AddCwMembersWithAttacks(ClanWarBuilder clanWarBuilder, ClanWarApi clanWarInfoFromApi)
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
                    var warMemberAttack = new WarAttackBuilder();

                    warMemberAttack.SetBaseProperties(warAttack);

                    warMemberAttack.SetWarMember(warMemberBuilder.WarMember);

                    warMemberAttack.SetEnemyWarMember(clanWarBuilder.ClanWar.EnemyWarMembers
                        .First(x => x.Tag == warAttack.DefenderTag));

                    warMemberAttacks.Add(warMemberAttack.WarAttack);
                }
            }

            warMemberBuilder.SetWarAttacks(warMemberAttacks);

            warMemberBuilder.SetClanWar(clanWarBuilder.ClanWar);

            var ClanMemberOnWar = TrackedClanBuilder.Clan.ClanMembers
               .FirstOrDefault(x => x.Tag == warMemberBuilder.WarMember.Tag);

            warMemberBuilder.SetClanMember(ClanMemberOnWar);

            if (ClanMemberOnWar != null)
            {
                ClanMemberOnWar.WarMemberships.Add(warMemberBuilder.WarMember);
            }

            warMembers.Add(warMemberBuilder.WarMember);
        }

        clanWarBuilder.SetWarMembers(warMembers);

        return clanWarBuilder;
    }
}
