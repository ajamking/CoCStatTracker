using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Storage;
using Telegram.Bot.Types.Enums;
using CoCStatsTrackerBot.Menue;

namespace CoCStatsTrackerBot;

public static class MemberRequestHandler
{
    private static List<BaseMenu> BaseMenus = Navigator.Menues;

    public static Regex PlayerRegex { get; set; } = new Regex(@"^#(\w{6,9})$");

    public static Regex ClanRegex { get; set; } = new Regex(@"^#(\w{8})$");

    public static Dictionary<long, string> LastUserPlayerTags { get; set; } = new Dictionary<long, string>();
    public static Dictionary<long, string> LastUserClanTags { get; set; } = new Dictionary<long, string>();

    public async static Task HandlePlayerInfoLvl2(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckMemberTagMessageExist(botClient, message);

        try
        {
            if (PlayerRegex.IsMatch(message.Text))
            {
                await CheckMemberExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.ShortPlayerInfo(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.FullPlayerInfo(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[2])
            {
                return;
            }
            if (message.Text == keywords[3])
            {
                return;
            }
            if (message.Text == keywords[4])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.MemberDrawMembership(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[5])
            {
                return;
            }
            if (message.Text == keywords[6])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.MemberCarmaHistory(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[7])
            {
                return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception in MemberRequestHandler" + e.Message);

            return;
        }
    }

    public async static Task HandleClanInfoLvl2(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckClanTagMessageExist(botClient, message);

        try
        {
            if (ClanRegex.IsMatch(message.Text))
            {
                await CheckClanExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                 text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[2])
            {
                return;
            }
            if (message.Text == keywords[3])
            {
                return;
            }
            if (message.Text == keywords[4])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[5])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[6])
            {
                return;
            }
            if (message.Text == keywords[7])
            {
                return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception in MemberRequestHandler" + e.Message);

            return;
        }
    }

    public async static Task HandleCurrentWarInfoLvl2(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckClanTagMessageExist(botClient, message);

        try
        {
            if (ClanRegex.IsMatch(message.Text))
            {
                await CheckClanExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                   text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[2])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
                return;
            }
            if (message.Text == keywords[3])
            {
                return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception in MemberRequestHandler" + e.Message);

            return;
        }
    }

    public async static Task HandleCurrentRaidInfoLvl2(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckClanTagMessageExist(botClient, message);

        try
        {
            if (ClanRegex.IsMatch(message.Text))
            {
                await CheckClanExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[2])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
            }
            if (message.Text == keywords[3])
            {
                return;
            }
            if (message.Text == keywords[4])
            {
                return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception in MemberRequestHandler" + e.Message);

            return;
        }

    }

    public async static Task HandleCurrentPrizeDrawInfoLvl2(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckClanTagMessageExist(botClient, message);

        try
        {
            if (ClanRegex.IsMatch(message.Text))
            {
                await CheckClanExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[2])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
            }
            if (message.Text == keywords[3])
            {
                return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception in MemberRequestHandler" + e.Message);

            return;
        }
    }


    public async static Task HandlePlayerWarStatisticsLvl3(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckMemberTagMessageExist(botClient, message);

        try
        {
            if (PlayerRegex.IsMatch(message.Text))
            {
                await CheckMemberExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.WarStatistics(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, 1),
                  parseMode: ParseMode.MarkdownV2);
                
                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                   text: MemberFunctions.WarStatistics(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, 3),
                   parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[2])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.WarStatistics(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, 5),
                  parseMode: ParseMode.MarkdownV2);
            }
            if (message.Text == keywords[3])
            {
                return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception in MemberRequestHandler" + e.Message);

            return;
        }
    }

    public async static Task HandlePlayerRaidStatisticsLvl3(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckMemberTagMessageExist(botClient, message);
        
        try
        {
            if (PlayerRegex.IsMatch(message.Text))
            {
                await CheckMemberExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.RaidStatistics(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, 1),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.RaidStatistics(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, 3),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[2])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.RaidStatistics(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, 5),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[3])
            {
                return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception in MemberRequestHandler" + e.Message);

            return;
        }
    }

    public async static Task HandlePlayerArmyLvl3(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckMemberTagMessageExist(botClient, message);

        try
        {
            if (PlayerRegex.IsMatch(message.Text))
            {
                await CheckMemberExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.MembersArmyInfo(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, UnitType.Hero),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.MembersArmyInfo(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, UnitType.SiegeMachine),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[2])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.MembersArmyInfo(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, UnitType.SuperUnit),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[3])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.MembersArmyInfo(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, UnitType.Unit),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[4])
            {
                return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception in MemberRequestHandler" + e.Message);

            return;
        }
    }

    public async static Task HandleClanWarHistoryLvl3(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckClanTagMessageExist(botClient, message);

        try
        {
            if (PlayerRegex.IsMatch(message.Text))
            {
                await CheckClanExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
               
                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[2])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[3])
            {
                return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception in MemberRequestHandler" + e.Message);

            return;
        }
    }

    public async static Task HandleClanRaidHistoryLvl3(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckClanTagMessageExist(botClient, message);

        try
        {
            if (PlayerRegex.IsMatch(message.Text))
            {
                await CheckClanExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[2])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[3])
            {
                return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception in MemberRequestHandler" + e.Message);

            return;
        }
    }

    public async static Task HandleClanPrizeDrawHistoryLvl3(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckClanTagMessageExist(botClient, message);

        try
        {
            if (PlayerRegex.IsMatch(message.Text))
            {
                await CheckClanExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[2])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[3])
            {
                return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception in MemberRequestHandler" + e.Message);

            return;
        }



    }

    public async static Task HandleCurrentDistrictStatistics3(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckClanTagMessageExist(botClient, message);

        try
        {
            if (PlayerRegex.IsMatch(message.Text))
            {
                await CheckClanExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[2])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[3])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[4])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[5])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[6])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[7])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);

                return;
            }
            if (message.Text == keywords[8])
            {
                return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception in MemberRequestHandler" + e.Message);

            return;
        }
    }


    public async static Task CheckMemberTagMessageExist(ITelegramBotClient botClient, Message message)
    {
        if (!LastUserPlayerTags.ContainsKey(message.Chat.Id))
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
            text: "Введите тег игрока в формате #123456789,\nа затем выберите пункт из меню");
        }
    }

    public async static Task CheckClanTagMessageExist(ITelegramBotClient botClient, Message message)
    {
        if (!LastUserClanTags.ContainsKey(message.Chat.Id))
        {
            var str = new StringBuilder("Введите один из тегов отслеживаемых кланов: ");

            foreach (var clan in Program.TrackedClans)
            {
                if (clan.IsCurrent)
                {
                    str.Append($"\n{clan.Name} - {clan.Tag} ");
                }
            }

            str.Append(", а затем выберите пункт из меню");

            await botClient.SendTextMessageAsync(message.Chat.Id,
            text: str.ToString());
        }
    }

    public async static Task CheckMemberExist(ITelegramBotClient botClient, Message message)
    {
        foreach (var clan in Program.TrackedClans.Where(x => x.IsCurrent == true))
        {
            try
            {
                var member = clan.ClanMembers.First(x => x.Tag == LastUserPlayerTags[message.Chat.Id]);
                await botClient.SendTextMessageAsync(message.Chat.Id,
                text: $"У меня есть информация об этом игроке. Что бы вы хотели узнать?");
                break;

            }
            catch (Exception e)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                text: $"Игрока с таки тегом нет в отслеживаемых кланах, попробуйте ввести тег повторно");
                break;
            }
        }
    }

    public async static Task CheckClanExist(ITelegramBotClient botClient, Message message)
    {
        foreach (var clan in Program.TrackedClans.Where(x => x.IsCurrent == true))
        {
            if (clan.Tag == message.Text)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                    text: $"У меня есть информация об этом клане. Что бы вы хотели узнать?");
                break;

            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
              text: $"Я не отслеживаю клан с таким тегом, попробуйте ввести тег повторно");
                break;
            }
        }
    }
}
