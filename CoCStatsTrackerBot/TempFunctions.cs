using CoCApiDealer.ApiRequests;
using CoCApiDealer;
using CoCStatsTracker.Helpers;
using Domain.Entities;
using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCStatsTrackerBot;
public static class TempFunctions
{
    public static void InitializeDb(string clanTag)
    {
        var TrackedClans = new DBInit(clanTag).TrackedClans;

        Console.WriteLine(@$"Two versions of {TrackedClans.First(x => x.Tag == clanTag).Name} clan added to DB {DateTime.Now} ");
    }

    public static void AddActivity()
    {
        using var db = new AppDbContext("Data Source=CoCStatsTracker.db");

        Program.TrackedClans = db.TrackedClans.ToList();

        foreach (var clan in Program.TrackedClans)
        {
            if (clan.IsCurrent == true)
            {
                clan.ClanMembers.FirstOrDefault(x => x.Tag == "#2VGG92CL9")
                .Carma
                .PlayerActivities.Add(
                new CustomActivity
                {
                    Name = "TestActivity123123",
                    EarnedPoints = 100,
                    UpdatedOn = DateTime.Now,
                    Description = "Тестирую как в кайф"
                });

                clan.ClanMembers.FirstOrDefault(x => x.Tag == "#G8P9Q299R")
                   .Carma
                   .PlayerActivities.Add(
                   new CustomActivity
                   {
                       Name = "TestActivity123123",
                       EarnedPoints = 20,
                       UpdatedOn = DateTime.Now,
                       Description = "Тестирую как в кайф"
                   });

                clan.ClanMembers.FirstOrDefault(x => x.Tag == "#RV9JP9Y")
                   .Carma
                   .PlayerActivities.Add(
                   new CustomActivity
                   {
                       Name = "TestActivity123123",
                       EarnedPoints = 15,
                       UpdatedOn = DateTime.Now,
                       Description = "Тестирую как в кайф"
                   });
            }

            foreach (var member in clan.ClanMembers)
            {
                foreach (var activity in member.Carma.PlayerActivities)
                {
                    member.Carma.TotalCarma += activity.EarnedPoints;
                }
            }
        }


        db.Complete();
    }

    public static void RecalculateDrawScores()
    {
        using var db = new AppDbContext("Data Source=CoCStatsTracker.db");
        Program.TrackedClans = db.TrackedClans.ToList();

        var first = Program.TrackedClans.FirstOrDefault(x => x.IsCurrent == true);
        var second = Program.TrackedClans.FirstOrDefault(x => x.IsCurrent == true);
        second.PrizeDraws.First().Members = DrawDealer.RecalculatePrizeDrawScores(first, second, second.PrizeDraws.First().Members);

        var Q12 = first.ClanMembers.FirstOrDefault(x => x.Tag == "#2VGG92CL9");

        db.Complete();
    }

    public static void GetCwMembers(string clanTag)
    {
        var cwRequest = new CurrentWarRequest();

        var keResult = cwRequest.CallApi(clanTag).Result;

        var str = new StringBuilder($"\n  {keResult.State}\n  {DateTimeParser.Parse(keResult.StartTime)} - {DateTimeParser.Parse(keResult.EndTime)}\n");

        var sortedClanMembers = keResult.ClanResults.WarMembers.OrderBy(x => x.MapPosition);

        var maxNameLenght = 0;

        foreach (var member in sortedClanMembers)
        {
            if (member.Name.Length > maxNameLenght)
            {
                maxNameLenght = member.Name.Length;
            }
        }

        foreach (var member in keResult.OpponentResults.WarMembers)
        {
            if (member.Name.Length > maxNameLenght)
            {
                maxNameLenght = member.Name.Length;
            }
        }

        foreach (var member in sortedClanMembers)
        {
            str.AppendLine($"  {Helper.CenteredString(member.Name, maxNameLenght)} ТХ {Helper.CenteredString(member.TownhallLevel.ToString(), 2)}" +
                $" Поз |{Helper.CenteredString(member.MapPosition.ToString(), 2)}| " +
                $"Th {keResult.OpponentResults.WarMembers.FirstOrDefault(x => x.MapPosition == member.MapPosition).TownhallLevel} " +
                $"{Helper.CenteredString(keResult.OpponentResults.WarMembers.FirstOrDefault(x => x.MapPosition == member.MapPosition).Name, maxNameLenght)}");
        }

        Console.WriteLine(str.ToString());
    }

    public static void GetNonAttackersCw(string clanTag)
    {
        var cwRequest = new CurrentWarRequest();

        var keResult = cwRequest.CallApi(clanTag).Result;

        var str = new StringBuilder("\n\n  Не провели атаки на КВ: ");

        var count = 0;

        foreach (var member in keResult.ClanResults.WarMembers)
        {
            if (member.Attacks == null)
            {
                str.AppendLine($"  {member.Name}");

                count++;
            }
        }

        Console.WriteLine(str);

        if (count == 0)
        {
            Console.WriteLine("  Все провели атаки на КВ, молодцы!");
        }
    }

    public static void GetNonAttackersRaids(string clanTag)
    {
        var raidRequest = new CapitalRaidsRequest();

        var keResult = raidRequest.CallApi(clanTag, 1).Result;

        var str = new StringBuilder("\n\n  Следующие игроки провели не все атаки на рейдах: \n\n");

        var count = 0;

        foreach (var raids in keResult.RaidsInfo)
        {


            foreach (var member in raids.RaidMembers)
            {
                if (member.AttacksCount != 6)
                {
                    str.AppendLine($"  {member.Name} Атак осталось {6 - member.AttacksCount}");

                    count++;
                }
            }
        }

        Console.WriteLine(str);

        if (count == 0)
        {
            Console.WriteLine("  Все провели атаки на рейдах, молодцы!");
        }
    }
}
