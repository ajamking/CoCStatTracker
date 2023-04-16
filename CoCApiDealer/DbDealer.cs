using CoCStatTracker;

namespace CoCApiDealer;
public class DbDealer
{
    public ICoCDbContext DbContext { get; set; }

    public DbDealer(ICoCDbContext dbContext)
    {
        DbContext = dbContext;
    }

    //public bool WriteClanInfo(string clanTag)
    //{
    //    var playerInfo = new PlayerRequest().CallApi(clanTag).Result;



    //}
}
