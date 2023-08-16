using CoCApiDealer.ApiRequests;
using CoCStatsTracker;
using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Builders;
using CoCStatsTracker.Helpers;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
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

        var obsoleteClanMembers = TrackedClanBuilder.Clan.ClanMembers.ToList();

        var updatedClanMembers = new List<ClanMember>();

        var SetMemberPropertyTasks = clanMembersFromApi.Select(async x =>
        {
            var playerInfoFromApi = await (new PlayerRequest().CallApi(x.Tag));

            var oldClanMember = obsoleteClanMembers.FirstOrDefault(x => x.Tag == playerInfoFromApi.Tag);

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

        var existingCurrentRaid = TrackedClanBuilder.Clan.CapitalRaids.FirstOrDefault(
            x => x.StartedOn == raidStartedOn);

        var raidBuilder = new CapitalRaidBuilder(existingCurrentRaid);

        raidBuilder.SetBaseProperties(raidInfoFromApi);

        raidBuilder.SetTrackedClan(TrackedClanBuilder.Clan);

        raidBuilder = AddRaidDefenses(raidBuilder, raidInfoFromApi);

        raidBuilder = AddRaidMembersWithAttacks(raidBuilder, raidInfoFromApi);

        raidBuilder = AddDefeatedClansAndRaidAttacks(raidBuilder, raidInfoFromApi);

        if (existingCurrentRaid == null)
        {
            TrackedClanBuilder.AddCapitalRaid(raidBuilder.Raid);
        }
        else
        {
            TrackedClanBuilder.Clan.CapitalRaids.Remove(existingCurrentRaid);
            TrackedClanBuilder.AddCapitalRaid(raidBuilder.Raid);
        }
    }

    //Добавляем защиты с рейдов.
    private CapitalRaidBuilder AddRaidDefenses(CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var raidDefenseBuilder = new RaidDefenseBuilder();

        raidDefenseBuilder.SetBaseProperties(raidInfoFromApi.RaidDefenses);

        raidDefenseBuilder.SetRaid(raidBuilder.Raid);

        raidBuilder.SetRaidDefenses(raidDefenseBuilder.Defenses);

        return raidBuilder;
    }

    //Добавляем атаки участников рейдов и связываем все сущности включающие RaidMembers.
    private CapitalRaidBuilder AddRaidMembersWithAttacks(CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var members = new List<RaidMember>();

        foreach (var member in raidInfoFromApi.RaidMembers)
        {
            var attacks = raidBuilder.Raid.RaidAttacks.Where(x => x.MemberTag == member.Tag).ToList();

            var raidMemberBuilder = new RaidMemberBuilder();

            raidMemberBuilder.SetBaseProperties(member);

            raidMemberBuilder.SetRaidMemberAttacks(attacks);

            raidMemberBuilder.SetRaid(raidBuilder.Raid);

            var clanMemberOnRaid = TrackedClanBuilder.Clan.ClanMembers
               .FirstOrDefault(x => x.Tag == raidMemberBuilder.Member.Tag);

            if (clanMemberOnRaid != null)
            {
                clanMemberOnRaid.RaidMemberships.Add(raidMemberBuilder.Member);
            }

            raidMemberBuilder.SetClanMember(clanMemberOnRaid);

            members.Add(raidMemberBuilder.Member);
        }

        raidBuilder.SetRaidMembers(members);

        return raidBuilder;
    }

    //Добавляем информацию о побежденных кланах и связываем атаки с атакуемым кланом.
    private CapitalRaidBuilder AddDefeatedClansAndRaidAttacks(CapitalRaidBuilder raidBuilder, RaidApi raidInfoFromApi)
    {
        var defeatedClans = new List<DefeatedClan>();

        var raidAttacks = new List<RaidAttack>();

        foreach (var defeatedClanApi in raidInfoFromApi.RaidOnClans)
        {
            var defeatedClanBuilder = new DefeatedClanBuilder();

            defeatedClanBuilder.SetBaseProperties(defeatedClanApi);

            defeatedClanBuilder.SetCapitalRaid(raidBuilder.Raid);

            var destroyedDistricts = new List<OpponentDistrict>();

            foreach (var defeatedDistrict in defeatedClanApi.DistrictsDestroyed)
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
                        var raidAttackBuilder = new RaidAttackBuilder();

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
    public void UpdateCurrentClanWar(bool isCwLWar, string cwlWarTag = "")
    {
        //Определяем тип войны
        var clanWarInfoFromApi = isCwLWar ?
            new CwlWarRequest().CallApi(cwlWarTag).Result :
            new CurrentWarRequest().CallApi(TrackedClanBuilder.Clan.Tag).Result;

        var cwStartedOn = DateTimeParser.Parse(clanWarInfoFromApi.StartTime);

        var existingCurrentCw = TrackedClanBuilder.Clan.ClanWars.FirstOrDefault(
           x => x.StartedOn == cwStartedOn);

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
            var enemyWarMemberBuilder = new EnemyWarMemberBuilder();

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
            var warMemberBuilder = new WarMemberBuilder();

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
