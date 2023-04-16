using CoCApiDealer.UIEntities;
using Storage;

public class Program
{
    /// <summary>
    /// #YPPGCCY8 - тег клана
    /// #2VGG92CL9 - тег игрока
    /// </summary>

    static async Task Main(string[] args)
    {
        //RunDb();

        //var requesResult = new PlayerRequest().CallApi("#2VGG92CL9").Result;

        //Console.WriteLine(requesResult.Tag);
        //Console.WriteLine(requesResult.Name);
        //Console.WriteLine(requesResult.TownHallLevel);
        //Console.WriteLine(requesResult.TownHallWeaponLevel);
        //Console.WriteLine(requesResult.DonationsSent);
        //Console.WriteLine(requesResult.DonationsReceived);
        //Console.WriteLine();
        //Console.WriteLine("Heroes:");
        //foreach (var item in requesResult.Heroes)
        //{
        //    Console.WriteLine(item.Name + " " + item.Level);
        //}
        //Console.WriteLine();





        var pl = new PlayerInfoUi() { Name = "afs", ClanName = "124" };

        Console.WriteLine(@$"{pl.Name} // {pl.Tag} // {pl.ClanName} // {pl.TownHallLevel} // {pl.TownHallWeaponLevel}");

        //var excd = new ExcelDealer();



        //excd.CreateExcelFile();

        //Console.WriteLine();
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
