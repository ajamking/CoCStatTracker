using CoCApiDealer.ApiRequests;
using CoCStatsTracker;
using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Builders;
using CoCStatsTracker.Builders.ManualControl;
using CoCStatsTracker.UIEntities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Storage;

namespace CoCApiDealer;

public class DaddyBuilder
{
    public TrackedClanBuilder _trackedClanBuilder { get; private set; } = new TrackedClanBuilder();

    private ClanApi _clanInfoFromApi = new ClanApi();

    //На случай, если создаем не с нуля.
    public DaddyBuilder(TrackedClanBuilder _existingTrackedClan = null)
    {
        if (_existingTrackedClan != null)
        {
            _trackedClanBuilder = _existingTrackedClan;
        }
    }

    //Подтянули из API информацию о клане, записали в TCBuilder.
    public void SetClanProperties(string clanTag)
    {
        _clanInfoFromApi = new ClanInfoRequest().CallApi(clanTag).Result;

        _trackedClanBuilder.SetBaseProperties(_clanInfoFromApi);

        SetClanMembers(_clanInfoFromApi);
    }

    //Поочереди подтянули из API информацию о каждом игроке, вернули.
    private void SetClanMembers(ClanApi _clanInfoFromApi)
    {
        var clanMembers = new List<ClanMember>();

        foreach (var member in _clanInfoFromApi.Members)
        {
            var playerInfoFromApi = new PlayerRequest().CallApi(member.Tag).Result;

            var clanMemberBuilder = new ClanMemberBuilder();

            clanMemberBuilder.SetBaseProperties(playerInfoFromApi);

            clanMemberBuilder.SetUnits(playerInfoFromApi.Troops, playerInfoFromApi.Heroes);

            clanMembers.Add(clanMemberBuilder.ClanMember);
        }

        _trackedClanBuilder.SetClanMembers(clanMembers);
    }

    //Подтянули информацию о последнем рейде, создали домейнный объект со всеми связями
    public void AddRaid(string clanTag)
    {
        var raidInfoFromApi = new CapitalRaidsRequest().CallApi(clanTag, 1).Result.RaidsInfo.First();

        var raidBuilder = new CapitalRaidBuilder();

        raidBuilder.SetBaseProperties(raidInfoFromApi);

        //Добавляем информацию о защитах
        var raidDefenseBuilder = new RaidDefenseBuilder();

        raidDefenseBuilder.SetBaseProperties(raidInfoFromApi.RaidDefenses);

        raidBuilder.SetRaidDefenses(raidDefenseBuilder.Defenses);

        //Добавляем информацию о побежденных кланах и рейд атаках
        var defeatedClans = new List<DefeatedClan>();

        var raidAttacks = new List<RaidAttack>();

        foreach (var defeatedClanApi in raidInfoFromApi.RaidOnClans)
        {
            var defeatedClanBuilder = new DefeatedClanBuilder();

            defeatedClanBuilder.SetBaseProperties(defeatedClanApi);

            var destroyedDistricts = new List<OpponentDistrict>();

            foreach (var defeatedDistrict in defeatedClanApi.DistrictsDestroyed)
            {
                var defeatedDistrictBuilder = new OpponentDistrictBuilder();

                defeatedDistrictBuilder.SetBaseProperties(defeatedDistrict);

                if (defeatedDistrict.MemberAttacks != null)
                {
                    foreach (var attack in defeatedDistrict.MemberAttacks)
                    {
                        var raidAttackBuilder = new RaidAttackBuilder();
                        raidAttackBuilder.SetBaseProperties(attack);

                        raidAttacks.Add(raidAttackBuilder.RaidAttack);

                        raidAttacks.Last().OpponentDistrict = defeatedDistrictBuilder.District;
                    }
                }

                destroyedDistricts.Add(defeatedDistrictBuilder.District);
            }

            defeatedClanBuilder.SetOpponentDistricts(destroyedDistricts);

            defeatedClans.Add(defeatedClanBuilder.Clan);
        }

        raidBuilder.SetDefeatedClans(defeatedClans);
        raidBuilder.SetAttacks(raidAttacks);

        // Добавляем RaidMembers-ов в клан и в игроков
        var members = new List<RaidMember>();

        foreach (var member in raidInfoFromApi.RaidMembers)
        {
            var raidMemberBuilder = new RaidMemberBuilder();

            raidMemberBuilder.SetBaseProperties(member);

            members.Add(raidMemberBuilder.Member);
        }

        foreach (var member in _trackedClanBuilder.Clan.ClanMembers)
        {
            var clanMemberBuilder = new ClanMemberBuilder(member);

            if (members.FirstOrDefault(x => x.Tag == member.Tag) != null)
            {
                clanMemberBuilder.AddRaidMembership(
                    members.FirstOrDefault(x => x.Tag == member.Tag));
            }

        }

        raidBuilder.SetRaidMembers(members);

        _trackedClanBuilder.AddCapitalRaid(raidBuilder.Raid);
    }

    //Подтянули информацию о последней войне, создали домейнный объект со всеми связями
    public void AddClanWar(bool isCwLWar, string clanTag = "", string cwlWarTag = "")
    {
        ClanWarApi clanWarInfoFromApi;

        //Определяем тип войны
        if (isCwLWar)
        {
            clanWarInfoFromApi = new CwlWarRequest().CallApi(clanTag).Result;
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
            enemyWarmembers.Add(enemyWarMemberBuilder.EnemyWarMember);
        }

        clanWarBuilder.SetEnemyWarMembers(enemyWarmembers);

        //Создаем и заполняем участников войны вместе с их атаками и кладем
        //в ClanwarBuilder
        var warMembers = new List<WarMember>();

        foreach (var warMember in clanWarInfoFromApi.ClanResults.WarMembers)
        {
            var warMemberBuilder = new WarMemberBuilder();

            warMemberBuilder.SetBaseProperties(warMember);

            var warMemberAttacks = new List<WarAttack>();

            foreach (var warAttack in warMember.Attacks)
            {
                var warMemberAttack = new WarAttackBuilder();

                warMemberAttack.SetBaseProperties(warAttack);

                warMemberAttack.SetEnemyWarMember(clanWarBuilder.ClanWar.EnemyWarMembers
                    .FirstOrDefault(x => x.Tag == warAttack.DefenderTag));

                warMemberAttacks.Add(warMemberAttack.WarAttack);
            }

            warMemberBuilder.SetWarAttacks(warMemberAttacks);

            warMembers.Add(warMemberBuilder.WarMember);
        }

        clanWarBuilder.SetWarMembers(warMembers);

        _trackedClanBuilder.AddClanWar(clanWarBuilder.ClanWar);
    }

    //Присваиваем каждому игроку начальную карму
    public void AddEmptyCarmaToAllPlayers()
    {
        foreach (var member in _trackedClanBuilder.Clan.ClanMembers)
        {
            var emptyCarma = new CarmaBuilder();

            emptyCarma.SetBaseProperties();

            member.Carma = emptyCarma.PlayersCarma;
        }
    }

    public void AddPlayerActivity(string playerTag,
        string activityName, string description, int points)
    {
        var targetMember = _trackedClanBuilder.Clan
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
        var drawMembers = new List<DrawMember>();

        foreach (var clanMember in _trackedClanBuilder.Clan.ClanMembers)
        {
            var drawMemberBuilder = new DrawMemberBuilder();

            drawMemberBuilder.SetBaseProperties();

            clanMember.DrawMembership.Add(drawMemberBuilder.Member);
        }

        var prizeDrawBuilder = new PrizeDrawBuilder();

        prizeDrawBuilder.SetBaseProperties(start,end,desctiption);

        prizeDrawBuilder.SetDrawMembers(drawMembers);

        _trackedClanBuilder.AddPrizeDraw(prizeDrawBuilder.Draw);
    }
}
