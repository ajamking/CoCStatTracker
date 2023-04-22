using CoCApiDealer.UIEntities;
using CoCStatsTracker.UIEntities;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CoCStatsTracker;

public static class Mapper
{

    //
    //ClanInfoUi
    //
    public static ClanMemberUi MapToUi(ClanMember clanMember)
    {
        return new ClanMemberUi
        {
            Tag = clanMember.Tag,
            Name = clanMember.Name,
            RoleInClan = clanMember.Role,
            TownHallLevel = clanMember.TownHallLevel,
            DonationsSent = clanMember.DonationsSent,
            WarStars = clanMember.WarStars,
            CapitalContributions = clanMember.TotalCapitalContributions
        };
    }

    public static ClanUi MapToUi(TrackedClan clan)
    {
        var warLogType = "";

        if (clan.IsWarLogPublic == false)
            warLogType = "Закрытая";
        else
            warLogType = "Общедоступная";

        return new ClanUi
        {
            Tag = clan.Tag,
            Name = clan.Name,
            Type = clan.Type,
            Description = clan.Description,
            ClanLevel = clan.ClanLevel,
            ClanMembersCount = clan.ClanMembers.Count(),
            ClanPoints = clan.ClanPoints,
            ClanVersusPoints = clan.ClanVersusPoints,
            ClanCapitalPoints = clan.ClanCapitalPoints,
            CapitalLeague = clan.CapitalLeague,
            IsWarLogPublic = warLogType,
            WarLeague = clan.WarLeague,
            WarWinStreak = clan.WarWinStreak,
            WarWins = clan.WarWins,
            WarTies = clan.WarTies,
            WarLoses = clan.WarLoses,
            CapitalHallLevel = clan.CapitalHallLevel
        };
    }

    public static CwCwlUi MapToUi(ClanWar clanWar)
    {
        var warAttacks = new List<ClanWarAttackUi>();

        foreach (var member in clanWar.WarMembers)
        {
            var PlayerPerfomance = new ClanWarAttackUi { PlayerName = member.Name };
            var counter = 0;

            foreach (var attack in member.WarAttacks)
            {
                if (counter == 0)
                {
                    PlayerPerfomance.FirstStarsCount = attack.Stars;
                    PlayerPerfomance.FirstDestructionPercent = attack.DestructionPercent;
                }
                else
                {
                    PlayerPerfomance.SecondStarsCount = attack.Stars;
                    PlayerPerfomance.SecondStarsCount = attack.DestructionPercent;
                }
                counter++;

            }

            warAttacks.Add(PlayerPerfomance);
        }

        return new CwCwlUi
        {
            StartedOn = clanWar.StartTime,
            EndedOn = clanWar.EndTime,
            WarMembersCount = clanWar.WarMembers.Count(),
            ClanTag = clanWar.TrackedClan.Tag,
            ClanName = clanWar.TrackedClan.Name,
            TotalStarsEarned = clanWar.StarsCount,
            DestructionPercentage = clanWar.DestructionPercentage,
            OpponentName = clanWar.OpponentClanName,
            OpponentTag = clanWar.OpponentClanTag,
            OpponentStarsCount = clanWar.OpponentStarsCount,
            OpponentDestructionPercentage = clanWar.OpponentDestructionPercentage,
            Result = clanWar.Result,
            WarAttacks = warAttacks,
        };
    }

    public static DrawUi MapToUi(PrizeDraw draw)
    {
        return new DrawUi
        {
            StartedOn = draw.StartedOn,
            EndedOn = draw.EndedOn,
            Winner = draw.Winner,
            WinnersTotalScore = draw.WinnerTotalScore
        };
    }

    public static PlayerSuperUnitsUi MapToUi(ICollection<Troop> units)
    {
        var activatedUnits = new List<SuperUnitUi>();

        foreach (var unit in units)
        {
            if (unit.SuperTroopIsActivated == true)
            {
                activatedUnits.Add(new SuperUnitUi { Name = unit.Name, Level = unit.Level });
            }
        }

        return new PlayerSuperUnitsUi
        {
            PlayerName = units.FirstOrDefault().ClanMember.Name,
            ActivatedSuperUnits = activatedUnits
        };
    }

    //public static RaidsUi MapToUi(CapitalRaid raid)
    //{
    //    var defenses = new List<RaidDefenseUi>();

    //    //Фиксируем сведения обо всех нападениях за одни рейды
    //    foreach (var defense in raid.RaidDefenses)
    //    {
    //        defenses.Add(new RaidDefenseUi
    //        {
    //            AttackersTag = defense.AttackerClanTag,
    //            AttackersName = defense.AttackerClanName,
    //            TotalAttacksCount = defense.TotalAttacksCount
    //        });
    //    }

    //    var defeatedDistricts = new List<DistrictUi>();

    //    //Проходимся по всем повервежным кланам. Для вывода на UI нам сведения о самих кланах
    //    //не нужны, но пришлось добавить уровень, чтобы раскрыть структуру данных domain-а
    //    foreach (var clan in raid.DefeatedClans)
    //    {
    //        foreach (var district in clan.DefeatedDistricts)
    //        {
    //            var attacksOnChosenDistrict = new List<AttackOnDistrictUi>();

    //            foreach (var attack in district.Attacks)
    //            {
    //                attacksOnChosenDistrict.Add(new AttackOnDistrictUi
    //                {
    //                    PlayerName = attack.RaidMember.ClanMember.Name,
    //                    PlayerTag = attack.RaidMember.ClanMember.Tag,
    //                    // DestructionPercentFrom = attack.DestructionPercentFrom, //Отказались пока от этой идеи
    //                    DestructionPercentTo = attack.DestructionPercentTo,
    //                });
    //            }

    //            defeatedDistricts.Add(new DistrictUi
    //            {
    //                DistrictName = district.Name,
    //                DistrictLevel = district.Level,
    //                Attacks = attacksOnChosenDistrict,
    //            });
    //        }
    //    }

    //    return new RaidsUi
    //    {
    //        State = raid.State,
    //        StartedOn = raid.StartedOn,
    //        EndedOn = raid.EndedOn,
    //        ClanTag = raid.TrackedClan.Tag,
    //        ClanName = raid.TrackedClan.Name,
    //        TotalCapitalLoot = raid.TotalLoot,
    //        DefeatedDistrictsCount = raid.EnemyDistrictsDestoyed,
    //        DefensiveReward = raid.DefenSiveReward,
    //        OffensiveReward = raid.OffensiveReward * 6,
    //        RaidsCompleted = raid.DefeatedClans.Count,
    //        Defenses = defenses,
    //        AttackedDistricts = defeatedDistricts,
    //    };
    //}

    //
    //ClanMemberInfoUi
    //
    public static ArmyUi MapToArmyUi(ICollection<Troop> troops)
    {
        var superUnits = new Dictionary<string, int>();
        var siegeMachines = new Dictionary<string, int>();
        var heroes = new Dictionary<string, int>();
        var pets = new Dictionary<string, int>();
        var units = new Dictionary<string, int>();

        foreach (var troop in troops)
        {
            switch (troop.Type)
            {
                case UnitType.SuperUnit:
                    superUnits.Add(troop.Name, troop.Level);
                    break;
                case UnitType.SiegeMachine:
                    siegeMachines.Add(troop.Name, troop.Level);
                    break;
                case UnitType.Hero:
                    heroes.Add(troop.Name, troop.Level);
                    break;
                case UnitType.Pet:
                    pets.Add(troop.Name, troop.Level);
                    break;
                default:
                    units.Add(troop.Name, troop.Level);
                    break;
            }
        }

        return new ArmyUi
        {
            SuperUnits = superUnits,
            SiegeMachines = siegeMachines,
            Heroes = heroes,
            Pets = pets,
            Units = units,
        };
    }

    public static CarmaUi MapToCarmaUi(ClanMember member)
    {
        var activities = new List<ActivityUi>();

        foreach (var activity in member.Carma.PlayerActivities)
        {
            activities.Add(new ActivityUi
            {
                Name = activity.Name,
                Description = activity.Description,
                EarnedPoints = activity.EarnedPoints,
                UpdatedOn = activity.UpdatedOn
            });
        }

        return new CarmaUi
        {
            PlayersName = member.Name,
            PlayersTag = member.Tag,
            TotalCarma = member.Carma.TotalCarma,
            Activities = activities,
        };

    }

    public static CwCwlMembershipUi MapToCwCwlMembershipUi(WarMember member)
    {
        var attacks = new List<WarAttackUi>();

        foreach (var attack in member.WarAttacks)
        {
            attacks.Add(new WarAttackUi
            {
                EnemyTag = attack.EnemyWarMember.Tag,
                EnemyName = attack.EnemyWarMember.Name,
                AttackOrder = attack.AttackOrder,
                Stars = attack.Stars,
                DestructionPercent = attack.DestructionPercent,
                Duration = attack.Duration,
                EnemyTHLevel = attack.EnemyWarMember.THLevel,
                EnemyMapPosition = attack.EnemyWarMember.MapPosition,
            });
        }

        return new CwCwlMembershipUi
        {
            Tag = member.Tag,
            Name = member.Name,
            ClanTag = member.ClanWar.TrackedClan.Tag,
            ClanName = member.ClanWar.TrackedClan.Tag,
            StartedOn = member.ClanWar.StartTime,
            EndedOn = member.ClanWar.EndTime,
            TownHallLevel = member.TownHallLevel,
            MapPosition = member.MapPosition,
            BestOpponentStars = member.BestOpponentStars,
            BestOpponentsTime = member.BestOpponentTime,
            BestOpponentsPercent = member.BestOpponentPercent,
            Attacks = attacks,
        };
    }

    public static DrawMembershipUi MapToDrawMembershipUi(DrawMember drawMember)
    {
        var target = drawMember.PrizeDraw.Members
            .OrderBy(x => x.TotalPointsEarned)
            .Select((x, i) => new { Position = i, x.Member.Name, x.TotalPointsEarned })
            .First(x => x.Name == drawMember.Member.Name);

        return new DrawMembershipUi
        {
            PlayersName = drawMember.Member.Name,
            PlayersTag = drawMember.Member.Tag,
            ClanName = drawMember.PrizeDraw.TrackedClan.Name,
            ClanTag = drawMember.PrizeDraw.TrackedClan.Tag,
            DrawTotalScore = drawMember.TotalPointsEarned,
            PositionInClan = target.Position,
        };

    }

    public static PlayerInfoUi MapToPlayerInfoUi(ClanMember clanMember)
    {
        return new PlayerInfoUi
        {
            Tag = clanMember.Tag,
            Name = clanMember.Name,
            ClanTag = clanMember.Clan.Tag,
            ClanName = clanMember.Clan.Name,
            RoleInClan = clanMember.Role,
            ExpLevel = clanMember.ExpLevel,
            TownHallLevel = clanMember.TownHallLevel,
            TownHallWeaponLevel = clanMember.TownHallWeaponLevel,
            Trophies = clanMember.Trophies,
            BestTrophies = clanMember.BestTrophies,
            League = clanMember.League,
            VersusTrophies = clanMember.VersusTrophies,
            BestVersusTrophies = clanMember.BestVersusTrophies,
            AttackWins = clanMember.AttackWins,
            DefenceWins = clanMember.DefenceWins,
            WarPreference = clanMember.WarPreference,
            DonationsSent = clanMember.DonationsSent,
            DonationsRecieved = clanMember.DonationsRecieved,
            WarStars = clanMember.WarStars,
            TotalCapitalContributions = clanMember.TotalCapitalContributions
        };
    }

    //public static RaidMembershipUi MapToRaidMembershipUi(RaidMember raidMember)
    //{
    //    var attacks = new List<RaidAttackUi>();

    //    foreach (var attack in raidMember.Attacks)
    //    {
    //        attacks.Add(new RaidAttackUi
    //        {
    //            DefendersTag = attack.OpponentDistrict.DefeatedClan.DefendersTag,
    //            DefendersName = attack.OpponentDistrict.DefeatedClan.DefendersName,
    //            DistrictName = attack.OpponentDistrict.Name,
    //            DistrictLevel = attack.OpponentDistrict.Level,
    //            //  DestructionPercentFrom = attack.DestructionPercentFrom, // Отказались пока от этой идеи, сложно реализовать
    //            DestructionPercentTo = attack.DestructionPercentTo,
    //        });
    //    }

    //    return new RaidMembershipUi
    //    {
    //        Tag = raidMember.ClanMember.Tag,
    //        Name = raidMember.ClanMember.Name,
    //        ClanTag = raidMember.Raid.TrackedClan.Tag,
    //        ClanName = raidMember.Raid.TrackedClan.Name,
    //        StartedOn = raidMember.Raid.StartedOn,
    //        EndedOn = raidMember.Raid.EndedOn,
    //        TotalLoot = raidMember.TotalLoot,
    //        Attacks = attacks,
    //    };
    //}

    public static ShortPlayerInfoUi MapToShortPlayerInfoUi(ClanMember clanMember)
    {
        return new ShortPlayerInfoUi
        {
            Tag = clanMember.Tag,
            Name = clanMember.Name,
            WarPreference = clanMember.WarPreference,
            DonationsSent = clanMember.DonationsSent,
            DonationsRecieved = clanMember.DonationsRecieved,
            WarStars = clanMember.WarStars,
            TotalCapitalContributions = clanMember.TotalCapitalContributions,
        };
    }
    //
    //PrizeDrawInfoUi
    //
    public static CurrentPrizeDrawUi MapToPrizeDrawUi(PrizeDraw prizeDraw)
    {
        var participants = new List<ParticipantsUi>();

        foreach (var participant in prizeDraw.Members)
        {
            participants.Add(new ParticipantsUi
            {
                Tag = participant.Member.Tag,
                Name = participant.Member.Name,
                Role = participant.Member.Role,
                WarStarsCount = participant.Member.WarStars,
                DonationsSentCount = participant.Member.DonationsSent,
                CapitalContributionsCount = participant.Member.TotalCapitalContributions,
                CarmaScore = participant.Member.Carma.TotalCarma,
                TotalDrawScore = participant.TotalPointsEarned,
            });
        }

        return new CurrentPrizeDrawUi
        {
            ClanTag = prizeDraw.TrackedClan.Tag,
            ClanName = prizeDraw.TrackedClan.Name,
            StartOn = prizeDraw.StartedOn,
            EndedOn = prizeDraw.EndedOn,
            Description = prizeDraw.Description,
            Participants = participants,
        };
    }

    public static ShortPrizeDrawUi MapToShortPrizeDrawUi(PrizeDraw prizeDraw)
    {
        var participants = new Dictionary<string, int>();

        foreach (var participant in prizeDraw.Members)
        {
            participants.Add(participant.Member.Name, participant.TotalPointsEarned);
        }

        return new ShortPrizeDrawUi
        {
            ClanTag = prizeDraw.TrackedClan.Tag,
            ClanName = prizeDraw.TrackedClan.Name,
            StartOn = prizeDraw.StartedOn,
            EndedOn = prizeDraw.EndedOn,
            Description = prizeDraw.Description,
            Participants = participants,
        };
    }

}
