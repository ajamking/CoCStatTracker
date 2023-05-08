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

namespace CoCStatsTrackerBot;

public static class MemberRequestHandler
{
    public static Regex PlayerRegex { get; set; } = new Regex(@"^#(\w{6,9})$");

    public static Regex ClanRegex { get; set; } = new Regex(@"^#(\w{8})$");

    public static Dictionary<long, string> LastUserPlayerTags { get; set; } = new Dictionary<long, string>();
    public static Dictionary<long, string> LastUserClanTags { get; set; } = new Dictionary<long, string>();

    public async static Task HandleMessageMemberLvl2(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
                text: "Меню",
                replyMarkup: Menu.MemberKeyboards2[KeyboardType.Member]);
    }

    public async static Task HandlePlayerInfoLvl3(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await CheckMemberTagMessageExist(botClient, message);

        if (justMenu)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
              text: "Меню",
              replyMarkup: Menu.MemberKeyboards3[KeyboardType.PlayerInfo]);
            return;
        }

        try
        {
            switch (message.Text)
            {
                case string msg when PlayerRegex.IsMatch(msg):
                    {
                        await CheckMemberExist(botClient, message);

                        return;
                    }

                case "Главное об игроке":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                   text: MemberFunctions.ShortPlayerInfo(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans),
                   parseMode: ParseMode.MarkdownV2);
                        return;
                    }
                case "Все об игроке":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.FullPlayerInfo(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans),
                   parseMode: ParseMode.MarkdownV2);
                        return;
                    }
                case "Показатели войн":
                    {
                        await HandlePlayerWarStatisticsLvl4(botClient, message, true);
                        return;
                    }
                case "Показатели рейдов":
                    {
                        await HandleRaidStatisticsLvl4(botClient, message, true);
                        return;
                    }
                case "Розыгрыш":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                text: MemberFunctions.MemberDrawMembership(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans),
                   parseMode: ParseMode.MarkdownV2);
                        return;
                    }
                case "Войска":
                    {
                        await HandleArmyLvl4(botClient, message, true);
                        return;
                    }
                case "История кармы":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: MemberFunctions.MemberCarmaHistory(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans),
                   parseMode: ParseMode.MarkdownV2);
                        return;
                    }

                default:

                    return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception" + e.Message + " in MemberRequestHandler HandlePlayerInfoLvl3");
            return;
        }
    }

    public async static Task HandleClanInfoLvl3(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await CheckClanTagMessageExist(botClient, message);

        if (justMenu)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
              text: "Меню",
              replyMarkup: Menu.MemberKeyboards3[KeyboardType.ClanInfo]);
            return;
        }

        try
        {
            switch (message.Text)
            {
                case string msg when ClanRegex.IsMatch(msg):
                    {
                        await CheckClanExist(botClient, message);

                        return;
                    }

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
                        await HandleClanWarHistoryLvl4(botClient, message, true);
                        return;
                    }
                case "История рейдов":
                    {
                        await HandleClanRaidHistoryLvl4(botClient, message, true);
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
                        await HandleClanPrizeDrawHistoryLvl4(botClient, message, true);
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
        await CheckClanTagMessageExist(botClient, message);

        if (justMenu)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
              text: "Меню",
              replyMarkup: Menu.MemberKeyboards3[KeyboardType.CurrentWarInfo]);
            return;
        }




        try
        {
            switch (message.Text)
            {
                case string msg when ClanRegex.IsMatch(msg):
                    {
                        await CheckClanExist(botClient, message);

                        return;
                    }

                case "Главное":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Показатели":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Карта":
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
            Console.WriteLine("Exception" + e.Message + " in MemberRequestHandler HandleCurrentWarInfoLvl3");
            return;
        }
    }

    public async static Task HandleCurrentRaidInfoLvl3(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {

        await CheckClanTagMessageExist(botClient, message);

        if (justMenu)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
             text: "Меню",
             replyMarkup: Menu.MemberKeyboards3[KeyboardType.CurrentRaidInfo]);
            return;
        }



        try
        {
            switch (message.Text)
            {
                case string msg when ClanRegex.IsMatch(msg):
                    {
                        await CheckClanExist(botClient, message);

                        return;
                    }

                case "Главное":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Показатели":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Средние показатели":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                                         text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Статистика по районам":
                    {
                        await HandleClanDistrictStatisticsLvl4(botClient, message, true);
                        return;
                    }

                default:

                    return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception" + e.Message + " in MemberRequestHandler HandleCurrentRaidInfoLvl3");
            return;
        }


    }

    public async static Task HandleCurrentPrizeDrawInfoLvl3(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await CheckClanTagMessageExist(botClient, message);


        if (justMenu)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
              text: "Меню",
              replyMarkup: Menu.MemberKeyboards3[KeyboardType.CurrentPrizedrawInfo]);
            return;
        }



        try
        {
            switch (message.Text)
            {
                case string msg when ClanRegex.IsMatch(msg):
                    {
                        await CheckClanExist(botClient, message);
                        return;
                    }

                case "Главное":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Показатели":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Описание":
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
            Console.WriteLine("Exception" + e.Message + " in MemberRequestHandler HandleCurrentPrizeDrawInfoLvl3");
            return;
        }



    }


    public async static Task HandlePlayerWarStatisticsLvl4(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await CheckMemberTagMessageExist(botClient, message);


        if (justMenu)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
            text: "Меню",
            replyMarkup: Menu.MemberKeyboards4[KeyboardType.PlayerWarStatistics]);
            return;
        }



        try
        {
            switch (message.Text)
            {
                case string msg when ClanRegex.IsMatch(msg):
                    {
                        await CheckMemberExist(botClient, message);

                        return;
                    }

                case "Последняя война":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                       text: MemberFunctions.WarStatistics(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, 1),
                   parseMode: ParseMode.MarkdownV2);
                        return;
                    }
                case "Последние 3":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: MemberFunctions.WarStatistics(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, 3),
                   parseMode: ParseMode.MarkdownV2);
                        return;
                    }
                case "Последние 5":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: MemberFunctions.WarStatistics(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, 5),
                   parseMode: ParseMode.MarkdownV2);
                        return;
                    }

                default:

                    return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception" + e.Message + " in MemberRequestHandler HandleWarStatisticsLvl4");
            return;
        }

    }

    public async static Task HandleClanWarHistoryLvl4(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await CheckClanTagMessageExist(botClient, message);

        if (justMenu)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
            text: "Меню",
            replyMarkup: Menu.MemberKeyboards4[KeyboardType.ClanWarsHistory]);
            return;
        }




        try
        {
            switch (message.Text)
            {
                case string msg when ClanRegex.IsMatch(msg):
                    {
                        await CheckClanExist(botClient, message);

                        return;
                    }

                case "Последняя война":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Последние 3":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Последние 5":
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
            Console.WriteLine("Exception" + e.Message + " in MemberRequestHandler HandleWarStatisticsLvl4");
            return;
        }



    }

    public async static Task HandleRaidStatisticsLvl4(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {

        await CheckMemberTagMessageExist(botClient, message);

        if (justMenu)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
            text: "Меню",
            replyMarkup: Menu.MemberKeyboards4[KeyboardType.PlayerRaidStatistics]);
            return;
        }



        try
        {
            switch (message.Text)
            {
                case string msg when ClanRegex.IsMatch(msg):
                    {
                        await CheckMemberExist(botClient, message);

                        return;
                    }

                case "Последний рейд":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: MemberFunctions.RaidStatistics(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, 1),
                   parseMode: ParseMode.MarkdownV2);
                        return;
                    }
                case "Последние 3":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: MemberFunctions.RaidStatistics(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, 3),
                   parseMode: ParseMode.MarkdownV2);
                        return;
                    }
                case "Последние 5":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                         text: MemberFunctions.RaidStatistics(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, 5),
                   parseMode: ParseMode.MarkdownV2);
                        return;
                    }

                default:

                    return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception" + e.Message + " in MemberRequestHandler HandleRaidStatisticsLvl4");
            return;
        }

    }

    public async static Task HandleClanRaidHistoryLvl4(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await CheckClanTagMessageExist(botClient, message);


        if (justMenu)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
            text: "Меню",
            replyMarkup: Menu.MemberKeyboards4[KeyboardType.ClanWarsHistory]);

            return;
        }



        try
        {
            switch (message.Text)
            {
                case string msg when ClanRegex.IsMatch(msg):
                    {
                        await CheckClanExist(botClient, message);

                        return;
                    }

                case "Последний рейд":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Последние 3":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Последние 5":
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
            Console.WriteLine("Exception" + e.Message + " in MemberRequestHandler HandleWarStatisticsLvl4");
            return;
        }




    }

    public async static Task HandleArmyLvl4(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {

        await CheckMemberTagMessageExist(botClient, message);


        if (justMenu)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
            text: "Меню",
            replyMarkup: Menu.MemberKeyboards4[KeyboardType.PlayerArmy]);
            return;
        }


        try
        {
            switch (message.Text)
            {
                case string msg when ClanRegex.IsMatch(msg):
                    {
                        await CheckMemberExist(botClient, message);

                        return;
                    }

                case "Герои игрока":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                       text: MemberFunctions.MembersArmyInfo(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, UnitType.Hero),
                   parseMode: ParseMode.MarkdownV2);
                        return;
                    }
                case "Осадные машины игрока":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                         text: MemberFunctions.MembersArmyInfo(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, UnitType.SiegeMachine),
                   parseMode: ParseMode.MarkdownV2);
                        return;
                    }
                case "Супер юниты игрока":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: MemberFunctions.MembersArmyInfo(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, UnitType.SuperUnit),
                   parseMode: ParseMode.MarkdownV2);
                        return;
                    }
                case "Все войска игрока":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                       text: MemberFunctions.MembersArmyInfo(LastUserPlayerTags[message.Chat.Id], Program.TrackedClans, UnitType.Unit),
                   parseMode: ParseMode.MarkdownV2);
                        return;
                    }

                default:

                    return;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception" + e.Message + " in MemberRequestHandler HandleArmyLvl4");
            return;
        }


    }

    public async static Task HandleClanPrizeDrawHistoryLvl4(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await CheckClanTagMessageExist(botClient, message);

        if (justMenu)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
            text: "Меню",
            replyMarkup: Menu.MemberKeyboards4[KeyboardType.ClanWarsHistory]);
            return;
        }




        try
        {
            switch (message.Text)
            {
                case string msg when ClanRegex.IsMatch(msg):
                    {
                        await CheckClanExist(botClient, message);

                        return;
                    }

                case "Последний розыгрыш":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Последние 3":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Последние 5":
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
            Console.WriteLine("Exception" + e.Message + " in MemberRequestHandler HandleWarStatisticsLvl4");
            return;
        }





    }

    public async static Task HandleClanDistrictStatisticsLvl4(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await CheckClanTagMessageExist(botClient, message);

        if (justMenu)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
            text: "Меню",
            replyMarkup: Menu.MemberKeyboards4[KeyboardType.ClanWarsHistory]);

            return;
        }



        try
        {
            switch (message.Text)
            {
                case string msg when ClanRegex.IsMatch(msg):
                    {
                        await CheckClanExist(botClient, message);

                        return;
                    }

                case "Столичный пик":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Лагерь варваров":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Долина колдунов":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Лагуна шаров":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Мастерская строителя":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Драконьи утесы":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Карьер големов":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "" /*Тут функция вывода*/);
                        return;
                    }
                case "Парк скелетов":
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
            Console.WriteLine("Exception" + e.Message + " in MemberRequestHandler HandleWarStatisticsLvl4");
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
                text: $"Игрока с таки тегом нет в клане {clan.Name}, попробуйте ввести тег повторно");
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
