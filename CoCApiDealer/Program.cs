using CoCApiDealer;
using CoCApiDealer.ApiRequests;
using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Builders;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Storage;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    /// <summary>
    /// #YPPGCCY8 - тег клана
    /// #2VGG92CL9 - тег игрока
    /// </summary>

    static async Task Main(string[] args)
    {
        Console.WriteLine(DateTime.Now);
        var daddyBuilder = new DaddyBuilder();
        daddyBuilder.SetClanProperties("#UQQGYJJP");
        Console.WriteLine(DateTime.Now);
        daddyBuilder.AddCurrentRaid(daddyBuilder._trackedClanBuilder.Clan.Tag);
        Console.WriteLine(DateTime.Now);
        daddyBuilder.AddCurrentClanWar(false, daddyBuilder._trackedClanBuilder.Clan.Tag);
        Console.WriteLine(DateTime.Now);
        daddyBuilder.AddEmptyCarmaToAllPlayers();
        Console.WriteLine(DateTime.Now);
        daddyBuilder.AddPrizeDraw(DateTime.Now, DateTime.Now.AddDays(30), "testPrizeDraw");
        Console.WriteLine(DateTime.Now);


        Console.WriteLine();

        RunDb(daddyBuilder._trackedClanBuilder.Clan);

        Console.WriteLine("ДБ успешно запущена");
    }

    static void RunDb(TrackedClan clan)
    {
        using (AppDbContext db = new AppDbContext("Data Source=CoCStatsTracker.db"))
        {


            db.TrackedClans.Add(clan);

            db.Complete();

            Console.WriteLine("Объекты успешно сохранены");

            // получаем объекты из бд и выводим на консоль
            var users = db.ClanMembers.ToList();

            Console.WriteLine("Список объектов:");

            foreach (var user in users)
            {
                Console.WriteLine($"{user.Id}.{user.Name} - {user.WarStars} - {user.TotalCapitalContributions}");
            }
        }
    }

}
