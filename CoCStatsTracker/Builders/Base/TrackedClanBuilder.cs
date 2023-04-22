using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace CoCStatsTracker.Builders;

public class TrackedClanBuilder
{
    public TrackedClan Clan { get; } = new TrackedClan();

    public TrackedClanBuilder(TrackedClan trackedClan = null)
    {
        if (trackedClan != null)
        {
            Clan = trackedClan;
        }
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
        Clan.CapitalLeague = clanApi.CapitalLeague.Name;
        Clan.IsWarLogPublic = clanApi.IsWarLogPublic;
        Clan.WarLeague = clanApi.WarLeague.Name;
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

    public void AddClanWar(ClanWar clanWar)
    {
        Clan.ClanWars.Add(clanWar);
    }

    public void AddCapitalRaid(CapitalRaid raid)
    {
        Clan.CapitalRaids.Add(raid);
    }

    public void AddPrizeDraw(PrizeDraw draw)
    {
        Clan.PrizeDraws.Add(draw);
    }
}
