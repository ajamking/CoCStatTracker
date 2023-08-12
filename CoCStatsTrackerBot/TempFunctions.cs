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

    public static void GetCwMembers(string clanTag)
    {
        var cwRequest = new CurrentWarRequest();

        var keResult = cwRequest.CallApi(clanTag).Result;

        var str = new StringBuilder($"\n```  {keResult.State}\n  {DateTimeParser.Parse(keResult.StartTime)} - {DateTimeParser.Parse(keResult.EndTime)}\n\n");

        var sortedClanMembers = keResult.ClanResults.WarMembers.OrderBy(x => x.MapPosition);

        var maxNameLenght = 0;

        foreach (var member in sortedClanMembers)
        {
            member.Name = UiHelper.ChangeInvalidSymbols(member.Name);

            if (member.Name.Length > maxNameLenght)
            {
                maxNameLenght = member.Name.Length;
            }
        }

        foreach (var member in keResult.OpponentResults.WarMembers)
        {
            member.Name = UiHelper.ChangeInvalidSymbols(member.Name);

            if (member.Name.Length > maxNameLenght)
            {
                maxNameLenght = member.Name.Length;
            }
        }

        foreach (var member in sortedClanMembers)
        {
            str.AppendLine($"  {UiHelper.GetCenteredString(member.Name, maxNameLenght)} ТХ {UiHelper.GetCenteredString(member.TownhallLevel.ToString(), 2)}" +
                $" Поз |{UiHelper.GetCenteredString(member.MapPosition.ToString(), 2)}| " +
                $"Th {keResult.OpponentResults.WarMembers.FirstOrDefault(x => x.MapPosition == member.MapPosition).TownhallLevel} " +
                $"{UiHelper.GetCenteredString(keResult.OpponentResults.WarMembers.FirstOrDefault(x => x.MapPosition == member.MapPosition).Name, maxNameLenght)}");
        }

        str.AppendLine("```");

        Console.WriteLine(str.ToString());
    }

    public static void GetNonAttackersCw(string clanTag)
    {
        var cwRequest = new CurrentWarRequest();

        var keResult = cwRequest.CallApi(clanTag).Result;

        var str = new StringBuilder("\n\n```  Не провели атаки на КВ: ");

        var count = 0;

        foreach (var member in keResult.ClanResults.WarMembers)
        {
            if (member.Attacks == null)
            {
                str.AppendLine($"  {UiHelper.ChangeInvalidSymbols(member.Name)}");

                count++;
            }
        }

        str.AppendLine("```");

        Console.WriteLine(str);

        if (count == 0)
        {
            Console.WriteLine("  Все провели атаки на КВ, молодцы!");
        }
    }

    public static void GetNonAttackersRaids(string clanTag)
    {
        var raidRequest = new CapitalRaidsRequest();

        var raidMembers = raidRequest.CallApi(clanTag, 1).Result.RaidsInfo.First().RaidMembers;

        var clanRequest = new ClanInfoRequest();

        var clanInfo = clanRequest.CallApi(clanTag).Result;

        var count = 0;

        var str = new StringBuilder($"\n\n```  Краткая информация об участниках последних рейдов \n");

        str.AppendLine($"\n  Следующие игроки не участвовали в рейдах в клане {clanInfo.Name}: \n");

        foreach (var member in clanInfo.Members)
        {
            if (!raidMembers.Any(x => x.Tag == member.Tag))
            {
                str.AppendLine($"  {UiHelper.ChangeInvalidSymbols(member.Name)} ");

                count++;
            }
        }

        str.AppendLine("\n  Следующие игроки провели не все атаки на рейдах: \n");

        foreach (var member in raidMembers)
        {
            if (member.AttacksCount != 6)
            {
                str.AppendLine($"  {UiHelper.ChangeInvalidSymbols(member.Name)} Атак осталось {6 - member.AttacksCount}");

                count++;
            }
        }
        str.AppendLine("```");

        Console.WriteLine(str);

        if (count == 0)
        {
            Console.WriteLine("  Все провели атаки на рейдах, молодцы!");
        }
    }
}
