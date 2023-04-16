using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System;

namespace CoCStatsTracker.Builders;

public class TrackedClanBuilder
{
    public TrackedClan TrackedClan { get; } = new TrackedClan();

    public TrackedClanBuilder(TrackedClan trackedClan = null)
    {
        if (trackedClan != null)
        {
            TrackedClan = trackedClan;
        }

    }

    public void SetBaseProperties(ClanApi clanApi)
    {
        TrackedClan.UpdatedOn = DateTime.Now; ;
        TrackedClan.Tag = clanApi.Tag;
        TrackedClan.Name = clanApi.Name;
        TrackedClan.Type = clanApi.Type;
        TrackedClan.Description = clanApi.Description;
        TrackedClan.ClanLevel = clanApi.ClanLevel;
        TrackedClan.ClanPoints = clanApi.ClanPoints;
        TrackedClan.ClanVersusPoints = clanApi.ClanVersusPoints;
        TrackedClan.ClanCapitalPoints = clanApi.ClanCapitalPoints;
        TrackedClan.CapitalLeague = clanApi.CapitalLeague.Name;
        TrackedClan.IsWarLogPublic = clanApi.IsWarLogPublic;
        TrackedClan.WarLeague = clanApi.WarLeague.Name;
        TrackedClan.WarWinStreak = clanApi.WarWinStreak;
        TrackedClan.WarWins = clanApi.WarWins;
        TrackedClan.WarTies = clanApi.WarTIes;
        TrackedClan.WarLoses = clanApi.WarLoses;
        TrackedClan.CapitalHallLevel = clanApi.CapitalHallLevel;
    }

    public void SetClanMembers()
    {

    }

    public void SetClanWars()
    {

    }

    public void SetCapitalRaids()
    {

    }

    public void SetPrizeDraw()
    {

    }

}
