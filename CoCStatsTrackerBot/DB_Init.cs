using CoCApiDealer;
using Domain.Entities;
using Storage;

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
        var daddyBuilder = new DaddyBuilder(new TrackedClan() { Tag = tag });

        daddyBuilder.UpdateClanBaseProperties();

        daddyBuilder.UpdateClanMembersBasePropertiesAndUnits();

        daddyBuilder.UpdateCurrentRaid();

        //Если вылетает null exc, возможно дело в устаревшем теге войны
        //daddyBuilder.AddCurrentClanWar(true, tag, "#88RPULYC9");
        //daddyBuilder.AddCurrentClanWar(true, tag, "#88RLJYULY");
        //daddyBuilder.AddCurrentClanWar(true, tag, "#88RRPGV99");
        //daddyBuilder.AddCurrentClanWar(true, tag, "#88RCVRYPV");
        //daddyBuilder.AddCurrentClanWar(true, tag, "#88J0QRJ9Q");
        //Закомментил тут, потому что не понял нафига тут именно так передается тег.
        //Если работает вариант с передачей просто тега из этой же функции.
        //daddyBuilder.AddCurrentClanWar(true, daddyBuilder.TrackedClanBuilder.Clan.Tag, "#88J98LL2C");

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
