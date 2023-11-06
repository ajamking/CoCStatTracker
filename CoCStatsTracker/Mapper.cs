using CoCStatsTracker.Items.Helpers;
using CoCStatsTracker.UIEntities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoCStatsTracker;

public static class Mapper
{
    //
    //ClanInfoUi
    //
    public static ClanUi MapToUi(TrackedClan clan)
    {
        var warLogType = "";

        if (clan.IsWarLogPublic == false)
            warLogType = "Закрытая";
        else
            warLogType = "Общедоступная";

        var clanMembers = new List<ClanMemberUi>(clan.ClanMembers.Count);

        foreach (var member in clan.ClanMembers)
        {
            clanMembers.Add(MapToUi(member));
        }

        return new ClanUi
        {
            UpdatedOn = clan.UpdatedOn,
            AdminsKey = clan.AdminsKey,
            IsInBlackList = clan.IsInBlackList,
            Tag = clan.Tag,
            Name = clan.Name,
            Type = clan.Type,
            Description = clan.Description,
            ClanLevel = clan.ClanLevel,
            ClanMembersCount = clan.ClanMembers.Count,
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
            CapitalHallLevel = clan.CapitalHallLevel,
            ClanMembers = clanMembers
        };
    }

    public static CwCwlUi MapToUi(ClanWar clanWar)
    {
        var warAttacks = new List<ClanWarAttackUi>();

        foreach (var member in clanWar.WarMembers)
        {
            var playerPerfomance = new ClanWarAttackUi
            {
                PlayerName = member.Name,
                ThLevel = member.TownHallLevel
            };

            var counter = 1;

            foreach (var attack in member.WarAttacks)
            {
                if (counter == 1)
                {
                    playerPerfomance.FirstEnemyThLevel = attack.EnemyWarMember.TownHallLevel;
                    playerPerfomance.FirstStarsCount = attack.Stars;
                    playerPerfomance.FirstDestructionPercent = attack.DestructionPercent;
                }
                else
                {
                    playerPerfomance.SecondEnemyThLevel = attack.EnemyWarMember.TownHallLevel;
                    playerPerfomance.SecondStarsCount = attack.Stars;
                    playerPerfomance.SecondDestructionpercent = attack.DestructionPercent;
                }

                counter++;
            }

            warAttacks.Add(playerPerfomance);
        }

        return new CwCwlUi
        {
            UpdatedOn = clanWar.UpdatedOn,
            PreparationStartTime = clanWar.PreparationStartTime,
            StartedOn = clanWar.StartedOn,
            EndedOn = clanWar.EndedOn,

            WarMembersCount = clanWar.WarMembers.Count(),
            AttackPerMember = clanWar.AttackPerMember,
            ClanTag = clanWar.TrackedClan.Tag,
            ClanName = clanWar.TrackedClan.Name,

            TotalStarsEarned = clanWar.StarsCount,
            DestructionPercentage = clanWar.DestructionPercentage,
            AttacksCount = clanWar.AttacksCount,

            OpponentName = clanWar.OpponentClanName,
            OpponentTag = clanWar.OpponentClanTag,
            OpponentStarsCount = clanWar.OpponentStarsCount,
            OpponentDestructionPercentage = clanWar.OpponentDestructionPercentage,
            OpponentAttacksCount = clanWar.OpponentAttacksCount,

            Result = clanWar.Result,
            MembersResults = warAttacks,

            WarMap = WarMapUiBuilder.Build(clanWar),
            NonAttackersCw = NonAttackersHelper.GetNonAttackersCw(clanWar)
        };
    }

    public static RaidUi MapToUi(CapitalRaid raid, TrackedClan trackedClan)
    {
        var defenses = new List<RaidDefenseUi>();

        //Фиксируем сведения обо всех защитах за одни рейды
        foreach (var defense in raid.RaidDefenses)
        {
            defenses.Add(new RaidDefenseUi
            {
                AttackersTag = defense.AttackerClanTag,
                AttackersName = defense.AttackerClanName,
                TotalAttacksCount = defense.TotalAttacksCount
            });
        }

        var allraidAttacks = new List<RaidAttack>();

        foreach (var raidMember in raid.RaidMembers)
        {
            allraidAttacks.AddRange(raidMember.Attacks);
        }

        var attackedClanTags = new HashSet<string>();

        var allDistrictNames = new HashSet<string>();

        foreach (var attack in allraidAttacks)
        {
            attackedClanTags.Add(attack.OpponentClanTag);

            allDistrictNames.Add(attack.OpponentDistrictName);
        }

        var defeatedClans = new List<DefeatedClanUi>();

        foreach (var clanTag in attackedClanTags)
        {
            var attacksOnClan = new List<RaidAttack>();

            foreach (var attack in allraidAttacks)
            {
                if (attack.OpponentClanTag == clanTag)
                {
                    attacksOnClan.Add(attack);
                }
            }

            var defeatedDistricts = new List<DistrictUi>();

            foreach (var districtName in allDistrictNames)
            {
                var attacksOnChosenDistrict = new List<AttackOnDistrictUi>();

                var distsrictLevel = 0;

                foreach (var attack in attacksOnClan)
                {
                    if (attack.OpponentDistrictName == districtName)
                    {
                        attacksOnChosenDistrict.Add(new AttackOnDistrictUi
                        {
                            PlayerTag = attack.MemberTag,
                            PlayerName = attack.MemberName,
                            DestructionPercentFrom = attack.DestructionPercentFrom,
                            DestructionPercentTo = attack.DestructionPercentTo,
                        });
                    }

                    distsrictLevel = attack.OpponentDistrictLevel;
                }

                defeatedDistricts.Add(new DistrictUi
                {
                    DistrictName = districtName,
                    DistrictLevel = distsrictLevel,
                    Attacks = attacksOnChosenDistrict,
                });
            }

            defeatedClans.Add(new DefeatedClanUi()
            {
                AttackedDistricts = defeatedDistricts,
                ClanTag = clanTag,
                ClanName = attacksOnClan.First().OpponentClanName,
                TotalAttacksCount = attacksOnClan.Count()
            });
        }

        return new RaidUi
        {
            UpdatedOn = raid.UpdatedOn,
            State = raid.State,
            StartedOn = raid.StartedOn,
            EndedOn = raid.EndedOn,
            ClanTag = raid.TrackedClan.Tag,
            ClanName = raid.TrackedClan.Name,
            TotalCapitalLoot = raid.TotalLoot,
            DefeatedDistrictsCount = raid.EnemyDistrictsDestoyed,
            DefensiveReward = raid.DefenSiveReward,
            OffensiveReward = raid.OffensiveReward,
            RaidsCompleted = defeatedClans.Count,
            Defenses = defenses,
            DefeatedClans = defeatedClans,
            NonAttackersRaids = NonAttackersHelper.GetNonAttackersRaids(trackedClan)
        };
    }

    //
    //ClanMemberInfoUi
    //
    public static ClanMemberUi MapToUi(ClanMember clanMember)
    {
        return new ClanMemberUi
        {
            UpdatedOn = clanMember.UpdatedOn,
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
            DefenseWins = clanMember.DefenceWins,
            WarPreference = clanMember.WarPreference,
            DonationsSent = clanMember.DonationsSent,
            DonationsRecieved = clanMember.DonationsRecieved,
            WarStars = clanMember.WarStars,
            TotalCapitalContributed = clanMember.TotalCapitalGoldContributed,
            TotalCapitalGoldLooted = clanMember.TotalCapitalGoldLooted,

            CwMedianDP = MedianValueCalculator.Calculate(clanMember, MedianValueType.ClanWar),
            CwMedianDPWithout14_15Th = MedianValueCalculator.Calculate(clanMember, MedianValueType.ClanWarWithout1415Th),
            RaidsMedianDP = MedianValueCalculator.Calculate(clanMember, MedianValueType.Raids),
            RaidsMedianDPWithoutPeak = MedianValueCalculator.Calculate(clanMember, MedianValueType.RaidsWithoutPeak)
        };
    }

    public static ArmyUi MapToUi(ICollection<Troop> troops, ClanMember clanMember)
    {
        var superUnits = new List<TroopUi>();
        var siegeMachines = new List<TroopUi>();
        var heroes = new List<TroopUi>();
        var pets = new List<TroopUi>();
        var units = new List<TroopUi>();

        foreach (var troop in troops)
        {
            switch (troop.Type)
            {
                case UnitType.SuperUnit:
                    superUnits.Add(new TroopUi
                    {
                        Name = troop.Name,
                        Lvl = troop.Level,
                        SuperTroopIsActivated = troop.SuperTroopIsActivated
                    });
                    break;

                case UnitType.SiegeMachine:
                    siegeMachines.Add(new TroopUi
                    {
                        Name = troop.Name,
                        Lvl = troop.Level,
                        SuperTroopIsActivated = troop.SuperTroopIsActivated
                    });
                    break;

                case UnitType.Hero:
                    heroes.Add(new TroopUi
                    {
                        Name = troop.Name,
                        Lvl = troop.Level,
                        SuperTroopIsActivated = troop.SuperTroopIsActivated
                    });
                    break;

                case UnitType.Pet:
                    pets.Add(new TroopUi
                    {
                        Name = troop.Name,
                        Lvl = troop.Level,
                        SuperTroopIsActivated = troop.SuperTroopIsActivated
                    });
                    break;

                default:
                    units.Add(new TroopUi
                    {
                        Name = troop.Name,
                        Lvl = troop.Level,
                        SuperTroopIsActivated = troop.SuperTroopIsActivated
                    });
                    break;
            }
        }

        return new ArmyUi
        {
            UpdatedOn = clanMember.UpdatedOn,
            SuperUnits = superUnits,
            SiegeMachines = siegeMachines,
            Heroes = heroes,
            Pets = pets,
            Units = units,
            PlayerName = clanMember.Name,
            PlayerTag = clanMember.Tag,
            TownHallLevel = clanMember.TownHallLevel,
            ClanTag = clanMember.Clan.Tag,
            ClanName = clanMember.Clan.Name,
        };
    }

    public static CwCwlMembershipUi MapToUi(WarMember member)
    {
        var attacks = new List<WarAttackUi>(member.WarAttacks.Count);

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
                EnemyTHLevel = attack.EnemyWarMember.TownHallLevel,
                EnemyMapPosition = attack.EnemyWarMember.MapPosition,
            });
        }

        return new CwCwlMembershipUi
        {
            UpdatedOn = member.UpdatedOn,
            Tag = member.Tag,
            Name = member.Name,
            ClanTag = member.ClanWar.TrackedClan.Tag,
            ClanName = member.ClanWar.TrackedClan.Name,
            PreparationStartedOn = member.ClanWar.PreparationStartTime,
            StartedOn = member.ClanWar.StartedOn,
            EndedOn = member.ClanWar.EndedOn,
            TownHallLevel = member.TownHallLevel,
            MapPosition = member.MapPosition,
            BestOpponentStars = member.BestOpponentStars,
            BestOpponentsTime = member.BestOpponentTime,
            BestOpponentsPercent = member.BestOpponentPercent,
            Attacks = attacks,
        };
    }

    public static RaidMembershipUi MapToUi(RaidMember raidMember)
    {
        var attacks = new List<RaidAttackUi>(raidMember.Attacks.Count());

        foreach (var attack in raidMember.Attacks)
        {
            attacks.Add(new RaidAttackUi
            {
                DefendersTag = attack.OpponentClanTag,
                DefendersName = attack.OpponentClanName,
                DistrictName = attack.OpponentDistrictName,
                DistrictLevel = attack.OpponentDistrictLevel,
                DestructionPercentFrom = attack.DestructionPercentFrom,
                DestructionPercentTo = attack.DestructionPercentTo,
            });
        }

        return new RaidMembershipUi
        {
            UpdatedOn = raidMember.UpdatedOn,
            Tag = raidMember.ClanMember.Tag,
            Name = raidMember.ClanMember.Name,
            ClanTag = raidMember.Raid.TrackedClan.Tag,
            ClanName = raidMember.Raid.TrackedClan.Name,
            StartedOn = raidMember.Raid.StartedOn,
            EndedOn = raidMember.Raid.EndedOn,
            TotalLoot = raidMember.TotalLoot,
            Attacks = attacks,
        };
    }

    public static AverageRaidsPerfomanceUi MapToUi(ICollection<RaidMember> raidMemberships, TrackedClan trackedClan)
    {
        var avgCapitalLoot = 0.0;
        var avgDestructionPercent = 0.0;
        var attackCounter = 0;

        var playerName = "";
        var playerTag = "";

        foreach (var raidMember in raidMemberships)
        {
            playerName = raidMember.MemberName;
            playerTag = raidMember.MemberTag;

            avgCapitalLoot += raidMember.TotalLoot;

            foreach (var attack in raidMember.Attacks)
            {
                avgDestructionPercent += (attack.DestructionPercentTo - attack.DestructionPercentFrom);
                attackCounter++;
            }
        }

        avgDestructionPercent /= attackCounter;

        avgCapitalLoot /= raidMemberships.Count();

        return new AverageRaidsPerfomanceUi
        {
            UpdatedOn = raidMemberships.FirstOrDefault().UpdatedOn,
            Tag = playerTag,
            Name = playerName,
            AverageDestructionPercent = Math.Round(avgDestructionPercent, 2),
            AverageCapitalLoot = Math.Round(avgCapitalLoot, 2),
            ClanName = trackedClan.Name,
            ClanTag = trackedClan.Tag
        };
    }

    public static SeasonStatisticsUi MapToUi(ClanMember currentClanMember, PreviousClanMember obsoleteClanMember, DateTime initializedOn)
    {
        return new SeasonStatisticsUi()
        {
            InitializedOn = initializedOn,
            UpdatedOn = currentClanMember.UpdatedOn,
            ClanName = currentClanMember.Clan.Name,
            ClanTag = currentClanMember.Clan.Tag,
            Name = currentClanMember.Name,
            Tag = currentClanMember.Tag,

            DonationsSend = currentClanMember.DonationsSent - obsoleteClanMember.DonationsSent,
            CapitalContributions = currentClanMember.TotalCapitalGoldContributed - obsoleteClanMember.TotalCapitalContributions,
            WarStarsEarned = currentClanMember.WarStars - obsoleteClanMember.WarStars
        };
    }
}