using CoCApiDealer;
using Domain.Entities;
using Storage;

namespace CoCStatsTrackerBot;

public class DBInit
{
    public List<TrackedClan> TrackedClans { get; }

    public DBInit(string clanTag, string clanTag2)
    {
        var currentClan = BuildClan(clanTag);
        currentClan.IsCurrent = true;

        var obsoleteClan = BuildClan(clanTag);
        obsoleteClan.IsCurrent = false;

        var currentClan2 = BuildClan(clanTag2);
        currentClan2.IsCurrent = true;

        var obsoleteClan2 = BuildClan(clanTag2);
        obsoleteClan2.IsCurrent = false;

        var clanRepresentations = new List<TrackedClan>() { currentClan, obsoleteClan, currentClan2, obsoleteClan2, };

        TrackedClans = RunDb(clanRepresentations);
    }

    public TrackedClan BuildClan(string tag)
    {
        var daddyBuilder = new DaddyBuilder(new TrackedClan() { Tag = tag });

        daddyBuilder.UpdateClanBaseProperties();

        daddyBuilder.UpdateClanMembersBasePropertiesAndUnits();

        daddyBuilder.UpdateCurrentRaid();

        //daddyBuilder.UpdateCurrentClanWar();

        return daddyBuilder.TrackedClanBuilder.Clan;
    }

    static List<TrackedClan> RunDb(ICollection<TrackedClan> clans)
    {
        using (AppDbContext db = new AppDbContext("Data Source=./../../../../CustomSolutionElements/CoCStatsTracker.db", true))
        {
            db.TrackedClans.AddRange(clans);

            db.Complete();

            return db.TrackedClans.ToList();
        }
    }

}
