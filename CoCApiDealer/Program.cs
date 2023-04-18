using CoCApiDealer.ApiRequests;
using CoCStatsTracker.Builders;
using Storage;

public class Program
{
    /// <summary>
    /// #YPPGCCY8 - тег клана
    /// #2VGG92CL9 - тег игрока
    /// </summary>

    static async Task Main(string[] args)
    {
        RunDb();

        var requesResult = new PlayerRequest().CallApi("#VUGGR9LR").Result;

        var clanMember = new ClanMemberBuilder();
        clanMember.SetBaseProperties(requesResult);
        clanMember.SetUnits(requesResult.Troops, requesResult.Heroes);

    }

    static void RunDb()
    {
        using (AppDbContext db = new AppDbContext("Data Source=CoCStatsTracker.db"))
        {
            //// создаем два объекта User
            //ClanMember tom = new ClanMember { Id = 1, Name = "Tom", WarStars = 33 };
            //ClanMember alice = new ClanMember { Id = 2, Name = "Alice", WarStars = 26 };

            //// добавляем их в бд
            //db.ClanMembers.Add(tom);
            //db.ClanMembers.Add(alice);
            //db.Complete();
            //Console.WriteLine("Объекты успешно сохранены");

            //// получаем объекты из бд и выводим на консоль
            //var users = db.ClanMembers.ToList();
            //Console.WriteLine("Список объектов:");
            //foreach (ClanMember u in users)
            //{
            //    Console.WriteLine($"{u.Id}.{u.Name} - {u.WarStars}");
            //}
        }
    }

}
