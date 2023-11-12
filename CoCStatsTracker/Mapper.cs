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
            ClanChatId = clan.ClansTelegramChatId,
            NewsLetterOn = clan.RegularNewsLetterOn,

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
            OpponentWarWinStreak = clanWar.OpponentWarWinStreak,
            OppinentWarDraws = clanWar.OppinentWarDraws,
            OppinentWarLoses = clanWar.OppinentWarLoses,
            OppinentWarWins = clanWar.OppinentWarWins,

            OpponentName = clanWar.OpponentsName,
            OpponentTag = clanWar.OpponentsTag,
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
        return new RaidUi
        {
            TotalAttacksCount = raid.TotalAttacks,
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
            Defenses = GetRaidDefensesUi(raid),
            DefeatedClans = GetAttackedClans(raid),
            NonAttackersRaids = NonAttackersHelper.GetNonAttackersRaids(trackedClan)
        };
    }

    //
    //ClanMemberInfoUi
    //
    public static ClanMemberUi MapToUi(ClanMember clanMember)
    {
        if (clanMember == null)
        {
            return new ClanMemberUi();
        }

        return new ClanMemberUi
        {
            UpdatedOn = clanMember.UpdatedOn,
            TelegramUserName = clanMember.TelegramUserName,

            Tag = clanMember.Tag,
            Name = clanMember.Name,
            ClanTag = clanMember.TrackedClan.Tag,
            ClanName = clanMember.TrackedClan.Name,
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
                    superUnits.Add(troop.MapToUi());
                    break;

                case UnitType.SiegeMachine:
                    siegeMachines.Add(troop.MapToUi());
                    break;

                case UnitType.Hero:
                    heroes.Add(troop.MapToUi());
                    break;

                case UnitType.Pet:
                    pets.Add(troop.MapToUi());
                    break;

                default:
                    units.Add(troop.MapToUi());
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
            ClanTag = clanMember.TrackedClan.Tag,
            ClanName = clanMember.TrackedClan.Name,
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
                DefendersTag = attack.AttackedClan.Tag,
                DefendersName = attack.AttackedClan.Name,
                DistrictName = attack.DefeatedEmemyDistrict.Name,
                DistrictLevel = attack.DefeatedEmemyDistrict.Level,
                DestructionPercentFrom = attack.DestructionPercentFrom,
                DestructionPercentTo = attack.DestructionPercentTo,
            });
        }

        return new RaidMembershipUi
        {
            UpdatedOn = raidMember.UpdatedOn,
            Tag = raidMember.ClanMember.Tag,
            Name = raidMember.ClanMember.Name,
            ClanTag = raidMember.CapitalRaid.TrackedClan.Tag,
            ClanName = raidMember.CapitalRaid.TrackedClan.Name,
            StartedOn = raidMember.CapitalRaid.StartedOn,
            EndedOn = raidMember.CapitalRaid.EndedOn,
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
            playerName = raidMember.Name;
            playerTag = raidMember.Tag;

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
            RaidMembershipsCount = raidMemberships.Count,
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
            ClanName = currentClanMember.TrackedClan.Name,
            ClanTag = currentClanMember.TrackedClan.Tag,
            Name = currentClanMember.Name,
            Tag = currentClanMember.Tag,

            DonationsSend = currentClanMember.DonationsSent - obsoleteClanMember.DonationsSent,
            DonationRecieved = currentClanMember.DonationsRecieved - obsoleteClanMember.DonationsRecieved,
            CapitalContributions = currentClanMember.TotalCapitalGoldContributed - obsoleteClanMember.TotalCapitalGoldContributed,
            WarStarsEarned = currentClanMember.WarStars - obsoleteClanMember.WarStars,
            AttackWins = currentClanMember.AttackWins - obsoleteClanMember.AttackWins,
            VersusBattleWins = currentClanMember.VersusBattleWins - obsoleteClanMember.VersusBattleWins
        };
    }



    private static TroopUi MapToUi(this Troop troop)
    {
        return new TroopUi
        {
            Village = troop.Village,
            Name = troop.Name,
            Lvl = troop.Level,
            SuperTroopIsActivated = troop.SuperTroopIsActivated
        };
    }

    private static List<RaidDefenseUi> GetRaidDefensesUi(CapitalRaid raid)
    {
        var defenses = new List<RaidDefenseUi>();

        foreach (var defense in raid.RaidDefenses)
        {
            var destroyedFriendlyDistricts = new List<DistrictUi>();

            foreach (var district in defense.DestroyedFriendlyDistricts)
            {
                destroyedFriendlyDistricts.Add(new DistrictUi()
                {
                    Name = district.Name,
                    Level = district.Level,
                    AttacksCount = district.AttacksSpent,
                    TotalDestructionPercent = district.TotalDestructionPersent
                });
            }

            defenses.Add(new RaidDefenseUi
            {
                EnemyClanTag = defense.AttackerClanTag,
                EnemyClanName = defense.AttackerClanName,
                TotalAttacksCount = defense.TotalAttacksCount,
                TotalEnemyLoot = defense.TotalEnemyLoot,
                DestroyedFriendlyDistrictsCount = defense.DestroyedFriendlyDistricts.Where(x => x.TotalDestructionPersent == 100).Count(),
                DestroyedFriendlyDistricts = destroyedFriendlyDistricts
            });
        }

        return defenses;
    }

    private static List<AttackedClanUi> GetAttackedClans(CapitalRaid raid)
    {
        var attackedClansUi = new List<AttackedClanUi>();

        foreach (var attackedClan in raid.AttackedClans)
        {
            var attackedClanUi = new AttackedClanUi()
            {
                Name = attackedClan.Name,
                Tag = attackedClan.Tag,
            };

            var defeatedEmemyDistricts = new List<DistrictUi>();

            foreach (var district in attackedClan.DefeatedEmemyDistricts)
            {
                var defeatedDistrictUi = new DistrictUi()
                {
                    Name = district.Name,
                    Level = district.Level,
                    Loot = district.TotalDistrictLoot,
                };

                var attacksOnDistrict = new List<AttackOnDistrictUi>();

                var totalDistrictDestructionPercent = 0;

                foreach (var attackOnDistrict in attackedClan.RaidAttacks.Where(x => x.DefeatedEmemyDistrict.Name == district.Name && x.DestructionPercentTo != 0))
                {
                    attacksOnDistrict.Add(new AttackOnDistrictUi()
                    {
                        AttackerName = attackOnDistrict.RaidMember.Name,
                        AttackerTag = attackOnDistrict.RaidMember.Tag,
                        
                        DestructionPercentFrom = attackOnDistrict.DestructionPercentFrom,
                        DestructionPercentTo = attackOnDistrict.DestructionPercentTo,
                    });

                    if (attackOnDistrict.DestructionPercentTo> totalDistrictDestructionPercent)
                    {
                        totalDistrictDestructionPercent = attackOnDistrict.DestructionPercentTo;
                    }
                }

                defeatedDistrictUi.Attacks = attacksOnDistrict;

                defeatedDistrictUi.AttacksCount = defeatedDistrictUi.Attacks.Count;

                defeatedDistrictUi.TotalDestructionPercent = totalDistrictDestructionPercent;

                defeatedEmemyDistricts.Add(defeatedDistrictUi);
            }

            attackedClanUi.DefeatedEmemyDistricts = defeatedEmemyDistricts;

            attackedClanUi.TotalAttacksCount = attackedClanUi.DefeatedEmemyDistricts.Sum(x => x.AttacksCount);

            attackedClanUi.TotalLoot = attackedClanUi.DefeatedEmemyDistricts.Sum(x => x.Loot);

            attackedClansUi.Add(attackedClanUi);
        }

        return attackedClansUi;
    }
}