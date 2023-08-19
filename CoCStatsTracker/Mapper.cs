using CoCApiDealer.UIEntities;
using CoCStatsTracker.UIEntities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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

        var clanMembers = new List<ClanMemberUi>();

        foreach (var member in clan.ClanMembers)
        {
            clanMembers.Add(MapToUi(member));
        }

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
            CapitalHallLevel = clan.CapitalHallLevel,
            ClanMembers = clanMembers
        };
    }

    public static CwCwlUi MapToUi(ClanWar clanWar)
    {
        var warAttacks = new List<ClanWarAttackUi>();

        foreach (var member in clanWar.WarMembers)
        {
            var playerPerfomance = new ClanWarAttackUi { PlayerName = member.Name };

            playerPerfomance.ThLevel = member.TownHallLevel;

            var counter = 1;

            foreach (var attack in member.WarAttacks)
            {
                if (counter == 1)
                {
                    playerPerfomance.FirstEnemyThLevel = attack.EnemyWarMember.THLevel;
                    playerPerfomance.FirstStarsCount = attack.Stars;
                    playerPerfomance.FirstDestructionPercent = attack.DestructionPercent;
                }
                else
                {
                    playerPerfomance.SecondEnemyThLevel = attack.EnemyWarMember.THLevel;
                    playerPerfomance.SecondStarsCount = attack.Stars;
                    playerPerfomance.SecondDestructionpercent = attack.DestructionPercent;
                }

                counter++;
            }

            warAttacks.Add(playerPerfomance);
        }

        return new CwCwlUi
        {

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
        };
    }

    public static RaidsUi MapToUi(CapitalRaid raid)
    {
        var defenses = new List<RaidDefenseUi>();

        //Фиксируем сведения обо всех нападениях за одни рейды
        foreach (var defense in raid.RaidDefenses)
        {
            defenses.Add(new RaidDefenseUi
            {
                AttackersTag = defense.AttackerClanTag,
                AttackersName = defense.AttackerClanName,
                TotalAttacksCount = defense.TotalAttacksCount
            });
        }

        var defeatedClans = new List<DefeatedClanUi>();

        foreach (var clan in raid.DefeatedClans)
        {
            var defeatedDistricts = new List<DistrictUi>();

            foreach (var district in clan.DefeatedDistricts)
            {
                var attacksOnChosenDistrict = new List<AttackOnDistrictUi>();

                foreach (var attack in district.Attacks)
                {
                    attacksOnChosenDistrict.Add(new AttackOnDistrictUi
                    {
                        PlayerName = attack.MemberName,
                        PlayerTag = attack.MemberTag,
                        DestructionPercentFrom = attack.DestructionPercentFrom,
                        DestructionPercentTo = attack.DestructionPercentTo,
                    });
                }

                defeatedDistricts.Add(new DistrictUi
                {
                    DistrictName = district.Name,
                    DistrictLevel = district.Level,
                    Attacks = attacksOnChosenDistrict,
                });
            }

            var attacsOnClanCount = 0;

            foreach (var district in defeatedDistricts)
            {
                attacsOnClanCount += district.Attacks.Count;
            }


            defeatedClans.Add(new DefeatedClanUi { ClanName = clan.DefendersName, ClanTag = clan.DefendersTag, AttackedDistricts = defeatedDistricts, TotalAttacksCount = attacsOnClanCount });

        }

        return new RaidsUi
        {
            State = raid.State,
            StartedOn = raid.StartedOn,
            EndedOn = raid.EndedOn,
            ClanTag = raid.TrackedClan.Tag,
            ClanName = raid.TrackedClan.Name,
            TotalCapitalLoot = raid.TotalLoot,
            DefeatedDistrictsCount = raid.EnemyDistrictsDestoyed,
            DefensiveReward = raid.DefenSiveReward,
            OffensiveReward = raid.OffensiveReward,
            RaidsCompleted = raid.DefeatedClans.Count,
            Defenses = defenses,
            DefeatedClans = defeatedClans,
        };
    }

    //
    //ClanMemberInfoUi
    //
    public static ArmyUi MapToArmyUi(ICollection<Troop> troops)
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
                        Lvl = troop.Level.ToString(),
                        SuperTroopIsActivated = troop.SuperTroopIsActivated.ToString()
                    });
                    break;

                case UnitType.SiegeMachine:
                    siegeMachines.Add(new TroopUi
                    {
                        Name = troop.Name,
                        Lvl = troop.Level.ToString(),
                        SuperTroopIsActivated = troop.SuperTroopIsActivated.ToString()
                    });
                    break;

                case UnitType.Hero:
                    heroes.Add(new TroopUi
                    {
                        Name = troop.Name,
                        Lvl = troop.Level.ToString(),
                        SuperTroopIsActivated = troop.SuperTroopIsActivated.ToString()
                    });
                    break;

                case UnitType.Pet:
                    pets.Add(new TroopUi
                    {
                        Name = troop.Name,
                        Lvl = troop.Level.ToString(),
                        SuperTroopIsActivated = troop.SuperTroopIsActivated.ToString()
                    });
                    break;

                default:
                    units.Add(new TroopUi
                    {
                        Name = troop.Name,
                        Lvl = troop.Level.ToString(),
                        SuperTroopIsActivated = troop.SuperTroopIsActivated.ToString()
                    });
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

    public static CwCwlMembershipUi MapToCwCwlMembershipUi(WarMember member)
    {
        var attacks = new List<WarAttackUi>();

        foreach (var attack in member.WarAttacks)
        {
            attacks.Add(new WarAttackUi
            {
                EnemyTag = attack.EnemyWarMember.Tag,
                EnemyName = attack.EnemyWarMember.Name,
                AttackOrder = attack.AttackOrder.ToString(),
                Stars = attack.Stars.ToString(),
                DestructionPercent = attack.DestructionPercent.ToString(),
                Duration = attack.Duration.ToString(),
                EnemyTHLevel = attack.EnemyWarMember.THLevel.ToString(),
                EnemyMapPosition = attack.EnemyWarMember.MapPosition.ToString(),
            });
        }

        var abc = new CwCwlMembershipUi();

        abc.Tag = member.Tag;
        abc.Name = member.Name;
        abc.ClanTag = member.ClanMember.Clan.Tag;
        abc.ClanName = member.ClanMember.Clan.Name;
        abc.StartedOn = member.ClanWar.StartedOn.ToString();
        abc.EndedOn = member.ClanWar.EndedOn.ToString();
        abc.TownHallLevel = member.TownHallLevel.ToString();
        abc.MapPosition = member.MapPosition.ToString();
        abc.BestOpponentStars = member.BestOpponentStars.ToString();
        abc.BestOpponentsTime = member.BestOpponentTime.ToString();
        abc.BestOpponentsPercent = member.BestOpponentPercent.ToString();
        abc.Attacks = attacks;

        return abc;

        //return new CwCwlMembershipUi
        //{
        //    Tag = member.Tag,
        //    Name = member.Name,
        //    ClanTag = member.ClanWar.TrackedClan.Tag,
        //    ClanName = member.ClanWar.TrackedClan.Name,
        //    StartedOn = member.ClanWar.StartedOn.ToString(),
        //    EndedOn = member.ClanWar.EndedOn.ToString(),
        //    TownHallLevel = member.TownHallLevel.ToString(),
        //    MapPosition = member.MapPosition.ToString(),
        //    BestOpponentStars = member.BestOpponentStars.ToString(),
        //    BestOpponentsTime = member.BestOpponentTime.ToString(),
        //    BestOpponentsPercent = member.BestOpponentPercent.ToString(),
        //    Attacks = attacks,
        //};
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
            DefenseWins = clanMember.DefenceWins,
            WarPreference = clanMember.WarPreference,
            DonationsSent = clanMember.DonationsSent,
            DonationsRecieved = clanMember.DonationsRecieved,
            WarStars = clanMember.WarStars,
            TotalCapitalContributions = clanMember.TotalCapitalContributions
        };
    }

    public static RaidMembershipUi MapToRaidMembershipUi(RaidMember raidMember)
    {
        var attacks = new List<RaidAttackUi>();

        foreach (var attack in raidMember.Attacks)
        {
            attacks.Add(new RaidAttackUi
            {
                DefendersTag = attack.OpponentDistrict.DefeatedClan.DefendersTag,
                DefendersName = attack.OpponentDistrict.DefeatedClan.DefendersName,
                DistrictName = attack.OpponentDistrict.Name,
                DistrictLevel = attack.OpponentDistrict.Level.ToString(),
                DestructionPercentFrom = attack.DestructionPercentFrom.ToString(),
                DestructionPercentTo = attack.DestructionPercentTo.ToString(),
            });
        }

        return new RaidMembershipUi
        {
            Tag = raidMember.ClanMember.Tag,
            Name = raidMember.ClanMember.Name,
            ClanTag = raidMember.Raid.TrackedClan.Tag,
            ClanName = raidMember.Raid.TrackedClan.Name,
            StartedOn = raidMember.Raid.StartedOn.ToString(),
            EndedOn = raidMember.Raid.EndedOn.ToString(),
            TotalLoot = raidMember.TotalLoot.ToString(),
            Attacks = attacks,
        };
    }

    public static AverageRaidsPerfomance MapToAverageRaidsPerfomance(ICollection<RaidMember> raidMemberships)
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

        return new AverageRaidsPerfomance
        {
            Tag = playerTag,
            Name = playerName,
            AverageDestructionPercent = Math.Round(avgDestructionPercent, 2),
            AverageCapitalLoot = Math.Round(avgCapitalLoot, 2)
        };
    }

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
}
