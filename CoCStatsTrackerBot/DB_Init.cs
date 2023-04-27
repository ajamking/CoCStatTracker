using CoCApiDealer;
using Domain.Entities;
using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCStatsTrackerBot;

public class DBInit
{
    public List<TrackedClan> TrackedClans { get; }

    public DBInit(string clanTag)
    {
        var currentClan = BuildClan(clanTag);
        currentClan.IsCurrent = true;

        var obsoleteClan = BuildClan(clanTag);
        obsoleteClan.IsCurrent = false;

        var clanRepresentations = new List<TrackedClan>() { currentClan, obsoleteClan };

        TrackedClans = RunDb(clanRepresentations);
    }


    public TrackedClan BuildClan(string tag)
    {
        var daddyBuilder = new DaddyBuilder();
        daddyBuilder.SetClanProperties(tag);

        daddyBuilder.AddCurrentRaid(daddyBuilder.TrackedClanBuilder.Clan.Tag);

        daddyBuilder.AddCurrentClanWar(false, daddyBuilder.TrackedClanBuilder.Clan.Tag);

        daddyBuilder.AddEmptyCarmaToAllPlayers();

        daddyBuilder.AddPrizeDraw(DateTime.Now, DateTime.Now.AddDays(30), "testPrizeDraw");

        return daddyBuilder.TrackedClanBuilder.Clan;
    }

    static List<TrackedClan> RunDb(ICollection<TrackedClan> clans)
    {
        using (AppDbContext db = new AppDbContext("Data Source=CoCStatsTracker.db", true))
        {
            db.TrackedClans.AddRange(clans);

            db.Complete();

            return db.TrackedClans.ToList();
        }
    }

}
