using CoCApiDealer.ApiRequests;
using CoCStatsTracker;
using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Builders;
using CoCStatsTracker.Builders.ManualControl;
using CoCStatsTracker.UIEntities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Storage;
using System.Diagnostics.Metrics;

namespace CoCApiDealer;

public class DaddyBuilder
{
    public TrackedClanBuilder TrackedClanBuilder { get; private set; } = new TrackedClanBuilder();

    private ClanApi _clanInfoFromApi = new ClanApi();

    //На случай, если создаем не с нуля.
    public DaddyBuilder(TrackedClanBuilder? _existingTrackedClan = null)
    {
        if (_existingTrackedClan != null)
        {
            TrackedClanBuilder = _existingTrackedClan;
        }
    }

    //Подтянули из API информацию о клане, записали в TCBuilder.
    public void SetClanProperties(string clanTag)
    {
        _clanInfoFromApi = new ClanInfoRequest().CallApi(clanTag).Result;

        TrackedClanBuilder.SetBaseProperties(_clanInfoFromApi);

        SetClanMembers(_clanInfoFromApi);
    }

    //Поочереди подтянули из API информацию о каждом игроке, вернули.
    private void SetClanMembers(ClanApi _clanInfoFromApi)
    {
        var clanMembers = new List<ClanMember>();

        var tasks = _clanInfoFromApi.Members.Select(async x =>
        {
            var playerInfoFromApi = await (new PlayerRequest().CallApi(x.Tag));

            var clanMemberBuilder = new ClanMemberBuilder();

            clanMemberBuilder.SetBaseProperties(playerInfoFromApi);

            clanMemberBuilder.SetUnits(playerInfoFromApi.Troops, playerInfoFromApi.Heroes);

            clanMemberBuilder.ClanMember.Clan = TrackedClanBuilder.Clan;

            clanMembers.Add(clanMemberBuilder.ClanMember);
        }).ToList();

        Task.WhenAll(tasks).GetAwaiter().GetResult();

        TrackedClanBuilder.SetClanMembers(clanMembers);
    }

    //Подтянули информацию о последнем рейде, создали домейнный объект со всеми связями
    public void AddCurrentRaid(string clanTag)
    {
        var raidInfoFromApi = new CapitalRaidsRequest().CallApi(clanTag, 1).Result.RaidsInfo.First();

        var raidBuilder = new CapitalRaidBuilder();

        raidBuilder.SetBaseProperties(raidInfoFromApi);

        raidBuilder.SetTrackedClan(TrackedClanBuilder.Clan);

        //Добавляем информацию о защитах
        var raidDefenseBuilder = new RaidDefenseBuilder();

        raidDefenseBuilder.SetBaseProperties(raidInfoFromApi.RaidDefenses);

        raidDefenseBuilder.SetRaid(raidBuilder.Raid);

        raidBuilder.SetRaidDefenses(raidDefenseBuilder.Defenses);

        // Добавляем RaidMembers-ов в клан и в игроков
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

            raidMemberBuilder.SetClanMember(clanMemberOnRaid);

            if (clanMemberOnRaid != null)
            {
                clanMemberOnRaid.RaidMemberships.Add(raidMemberBuilder.Member);
            }

            members.Add(raidMemberBuilder.Member);
        }

        raidBuilder.SetRaidMembers(members);

        //Добавляем информацию о побежденных кланах и рейд атаках
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

                        raidAttackBuilder.SetBaseProperties(attack, previousDestructionPercent);

                        raidAttackBuilder.SetRaidMember(raidBuilder.Raid.RaidMembers
                            .FirstOrDefault(x => x.Tag == attack.Attacker.Tag));

                        raidAttackBuilder.RaidAttack.OpponentDistrict = opponentDistrictBuilder.District;

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


        //foreach (var member in _trackedClanBuilder.Clan.ClanMembers)
        //{
        //    var clanMemberBuilder = new ClanMemberBuilder(member);

        //    if (members.FirstOrDefault(x => x.Tag == member.Tag) != null)
        //    {
        //        clanMemberBuilder.AddRaidMembership(
        //            members.FirstOrDefault(x => x.Tag == member.Tag));
        //    }

        //}

        

        TrackedClanBuilder.AddCapitalRaid(raidBuilder.Raid);
    }

    //Подтянули информацию о последней войне, создали домейнный объект со всеми связями
    public void AddCurrentClanWar(bool isCwLWar, string clanTag = "", string cwlWarTag = "")
    {
        ClanWarApi clanWarInfoFromApi;

        //Определяем тип войны
        if (isCwLWar)
        {
            clanWarInfoFromApi = new CwlWarRequest().CallApi(cwlWarTag).Result;
        }
        else
        {
            clanWarInfoFromApi = new CurrentWarRequest().CallApi(clanTag).Result;
        }

        //Устанавливаем основные поля
        var clanWarBuilder = new ClanWarBuilder();

        clanWarBuilder.SetBaseProperties(clanWarInfoFromApi);

        //Заполняем список противников
        var enemyWarmembers = new List<EnemyWarMember>();

        foreach (var enemyWarmember in clanWarInfoFromApi.OpponentResults.WarMembers)
        {
            var enemyWarMemberBuilder = new EnemyWarMemberBuilder();

            enemyWarMemberBuilder.SetBaseProperties(enemyWarmember);

            enemyWarMemberBuilder.SetClanWar(clanWarBuilder.ClanWar);

            enemyWarmembers.Add(enemyWarMemberBuilder.EnemyWarMember);
        }

        clanWarBuilder.SetEnemyWarMembers(enemyWarmembers);

        //Создаем и заполняем участников войны вместе с их атаками и кладем
        //в ClanwarBuilder
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

        TrackedClanBuilder.AddClanWar(clanWarBuilder.ClanWar);
    }

    //Присваиваем каждому игроку начальную карму
    public void AddEmptyCarmaToAllPlayers()
    {
        foreach (var member in TrackedClanBuilder.Clan.ClanMembers)
        {
            var emptyCarma = new CarmaBuilder();

            emptyCarma.SetBaseProperties();

            member.Carma = emptyCarma.PlayersCarma;
        }
    }

    public void AddPlayerActivity(string playerTag,
        string activityName, string description, int points)
    {
        var targetMember = TrackedClanBuilder.Clan
          .ClanMembers.FirstOrDefault(x => x.Tag == playerTag);

        if (targetMember != null)
        {
            var tempCarma = targetMember.Carma;

            var carmaBuilder = new CarmaBuilder(tempCarma);

            carmaBuilder.SetActivity(activityName, description, points);

            targetMember.Carma = carmaBuilder.PlayersCarma;
        }
        else
        {
            throw new ArgumentNullException(nameof(playerTag));
        }
    }

    //Добавляем отслеживаемый розыгрыш
    public void AddPrizeDraw(DateTime start, DateTime end, string desctiption)
    {
        var prizeDrawBuilder = new PrizeDrawBuilder();

        prizeDrawBuilder.SetBaseProperties(start, end, desctiption);

        var drawMembers = new List<DrawMember>();

        foreach (var clanMember in TrackedClanBuilder.Clan.ClanMembers)
        {
            var drawMemberBuilder = new DrawMemberBuilder();

            drawMemberBuilder.SetBaseProperties();

            drawMemberBuilder.SetClanMember(clanMember);

            drawMemberBuilder.SetDraw(prizeDrawBuilder.Draw);

            clanMember.DrawMemberships.Add(drawMemberBuilder.Member);

            drawMembers.Add(drawMemberBuilder.Member);
        }

        prizeDrawBuilder.SetDrawMembers(drawMembers);

        prizeDrawBuilder.SetTrackedClan(TrackedClanBuilder.Clan);

        TrackedClanBuilder.AddPrizeDraw(prizeDrawBuilder.Draw);
    }
}
