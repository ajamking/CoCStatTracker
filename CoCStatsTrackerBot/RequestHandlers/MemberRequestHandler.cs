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

namespace CoCStatsTrackerBot;

public static class MemberRequestHandler
{
    public static List<TrackedClan> TrackedClans { get; set; } = new List<TrackedClan>();

    public static Regex PlayerRegex { get; set; } = new Regex(@"^#(\w{9})$");

    public static Regex ClanRegex { get; set; } = new Regex(@"^#(\w{8})$");

    public static Dictionary<long, string> LastUserPlayerTags { get; set; } = new Dictionary<long, string>();
    public static Dictionary<long, string> LastUserClanTags { get; set; } = new Dictionary<long, string>();

    static MemberRequestHandler()
    {
        using var db = new AppDbContext("Data Source=CoCStatsTracker.db");

        TrackedClans = db.TrackedClans.ToList();

        db.Complete();

        Console.WriteLine("Connection winh DB in MemberRequestHandler sucsessful");
    }

    public async static Task HandleMessageMemberLvl2(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
                text: "Меню",
                replyMarkup: Menu.MemberKeyboards2[KeyboardType.Member]);
    }


    public async static Task HandlePlayerInfoLvl3(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
               text: "Меню",
               replyMarkup: Menu.MemberKeyboards3[KeyboardType.PlayerInfo]);

        if (!LastUserPlayerTags.ContainsKey(message.Chat.Id))
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
            text: "Введите тег игрока в формате #123456789,\nа затем выберите пункт из меню");
        }

        try
        {
            switch (message.Text)
            {
                case "Главное об игроке":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                   text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Все об игроке":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Показатели войн":
                    {
                        await HandleWarStatisticsLvl4(botClient, message);
                        return;
                    }
                case "Показатели рейдов":
                    {
                        await HandleRaidStatisticsLvl4(botClient, message);
                        return;
                    }
                case "Розыгрыш":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                 text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Войска":
                    {
                        await HandleArmyLvl4(botClient, message);
                        return;
                    }
                case "История кармы":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
                        return;
                    }

                default:

                    return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception" + e.Message + " in MemberRequestHandler HandleClanInfoLvl2");
            return;
        }
    }

    public async static Task HandleClanInfoLvl3(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
               text: "Меню",
               replyMarkup: Menu.MemberKeyboards3[KeyboardType.ClanInfo]);

        if (!LastUserClanTags.ContainsKey(message.Chat.Id))
        {
            var str = "";

            foreach (var clan in TrackedClans)
            {
                if (clan.IsCurrent)
                {
                    str += $"\n{clan.Name} - {clan.Tag} ";
                }
            }

            await botClient.SendTextMessageAsync(message.Chat.Id,

            text: "Введите один из тегов отслеживаемых кланов: " + str + ", а затем выберите пункт из меню");
        }

        try
        {
            switch (message.Text)
            {
                case "Главное о клане":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Члены клана":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "История войн":
                    {
                        await HandleWarHistoryLvl4(botClient, message);
                        return;
                    }
                case "История рейдов":
                    {
                        await HandleRaidHistoryLvl4(botClient, message);
                        return;
                    }
                case "Осадные машины":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                 text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Активные супер юниты":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                 text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "История розыгрышей":
                    {
                        await HandlePrizeDrawHistoryLvl4(botClient, message);
                        return;
                    }

                default:

                    return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception" + e.Message + " in MemberRequestHandler HandleClanInfoLvl3");
            return;
        }
    }

    public async static Task HandleCurrentWarInfoLvl3(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
               text: "Меню",
               replyMarkup: Menu.MemberKeyboards3[KeyboardType.CurrentWarInfo]);

        if (!LastUserClanTags.ContainsKey(message.Chat.Id))
        {
            var str = "";

            foreach (var clan in TrackedClans)
            {
                if (clan.IsCurrent)
                {
                    str += $"\n{clan.Name} - {clan.Tag} ";
                }
            }

            await botClient.SendTextMessageAsync(message.Chat.Id,

            text: "Введите один из тегов отслеживаемых кланов: " + str + ", а затем выберите пункт из меню");
        }

        try
        {
            switch (message.Text)
            {
                case "Главное о клане":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Члены клана":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "История войн":
                    {
                        await HandleWarHistoryLvl4(botClient, message);
                        return;
                    }
                case "История рейдов":
                    {
                        await HandleRaidHistoryLvl4(botClient, message);
                        return;
                    }
                case "Осадные машины":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                 text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Активные супер юниты":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                 text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "История розыгрышей":
                    {
                        await HandlePrizeDrawHistoryLvl4(botClient, message);
                        return;
                    }

                default:

                    return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception" + e.Message + " in MemberRequestHandler HandleClanInfoLvl3");
            return;
        }
    }

    public async static Task HandleCurrentRaidInfoLvl3(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {



    }

    public async static Task HandleCurrentPrizeDrawInfoLvl3(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {



    }


    public async static Task HandleWarStatisticsLvl4(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {


    }

    public async static Task HandleWarHistoryLvl4(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {



    }

    public async static Task HandleRaidStatisticsLvl4(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {


    }

    public async static Task HandleRaidHistoryLvl4(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {


    }

    public async static Task HandleArmyLvl4(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {


    }

    public async static Task HandlePrizeDrawHistoryLvl4(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {



    }

    public async static Task HandleDistrictStatisticsLvl4(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {



    }

}
