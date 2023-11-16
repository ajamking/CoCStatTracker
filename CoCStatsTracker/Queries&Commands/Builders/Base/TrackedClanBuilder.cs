using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace CoCStatsTracker.Builders;

public class TrackedClanBuilder
{
    public TrackedClan Clan { get; }

    public TrackedClanBuilder(TrackedClan trackedClan = null)
    {
        Clan = trackedClan ?? new TrackedClan();
    }

    public void SetBaseProperties(ClanApi clanApi)
    {
        Clan.UpdatedOn = DateTime.Now;
        Clan.Tag = clanApi.Tag;
        Clan.Name = clanApi.Name;
        Clan.Type = clanApi.Type;
        Clan.Description = clanApi.Description;
        Clan.ClanLevel = clanApi.ClanLevel;
        Clan.ClanPoints = clanApi.ClanPoints;
        Clan.ClanVersusPoints = clanApi.ClanVersusPoints;
        Clan.ClanCapitalPoints = clanApi.ClanCapitalPoints;
        Clan.CapitalLeague = clanApi.GetLeagueRU(ClanLeagueType.ClanCapitalLeague);
        Clan.IsWarLogPublic = clanApi.IsWarLogPublic;
        Clan.WarLeague = clanApi.GetLeagueRU(ClanLeagueType.ClanWarLeague);
        Clan.WarWinStreak = clanApi.WarWinStreak;
        Clan.WarWins = clanApi.WarWins;
        Clan.WarTies = clanApi.WarTIes;
        Clan.WarLoses = clanApi.WarLoses;
        Clan.CapitalHallLevel = clanApi.ClanCapital.CapitalHallLevel;
    }

    public void SetClanMembers(ICollection<ClanMember> members)
    {
        Clan.ClanMembers = members;
    }

    public void SetLastClanMembersStaticstics(ICollection<ClanMember> members)
    {
        var newPreviousClanMembers = new List<PreviousClanMember>();

        foreach (var member in members)
        {
            newPreviousClanMembers.Add(new PreviousClanMember()
            {
                UpdatedOn = DateTime.Now,
                TownHallLevel = member.TownHallLevel,
                TownHallWeaponLevel = member.TownHallWeaponLevel,
                Tag = member.Tag,
                Name = member.Name,
                ExpLevel = member.ExpLevel,
                Trophies = member.Trophies,
                BestTrophies = member.BestTrophies,
                WarStars = member.WarStars,
                AttackWins = member.AttackWins,
                DefenceWins = member.DefenceWins,
                BuilderHallLevel = member.BuilderHallLevel,
                VersusTrophies = member.VersusTrophies,
                BestVersusTrophies = member.BestVersusTrophies,
                VersusBattleWins = member.VersusBattleWins,
                Role = member.Role,
                WarPreference = member.WarPreference,
                DonationsSent = member.DonationsSent,
                DonationsRecieved = member.DonationsRecieved,
                TotalCapitalGoldContributed = member.TotalCapitalGoldContributed,
                TotalCapitalGoldLooted = member.TotalCapitalGoldLooted,
                League = member.League,
            });
        }

        Clan.PreviousClanMembersStaticstics = newPreviousClanMembers;
    }

    public void AddClanWar(ClanWar clanWar)
    {
        Clan.ClanWars.Add(clanWar);
    }

    public void AddCapitalRaid(CapitalRaid raid)
    {
        Clan.CapitalRaids.Add(raid);
    }
}