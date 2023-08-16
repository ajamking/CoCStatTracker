﻿using Domain.Entities;
using System.Text;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot;

public static class MemberRequestHandler
{
    private static string messageSplitToken = "answerReservedSplitter";

    public static Regex TagRegex { get; set; } = new Regex(@"^#(\w{6,9})$");

    public static Dictionary<long, string> UsersLastTags { get; set; } = new Dictionary<long, string>();

    public async static Task HandlePlayerInfoLvl2(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckMemberTagMessageExist(botClient, message);

        try
        {
            if (TagRegex.IsMatch(message.Text))
            {
                await CheckMemberExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: PlayerFunctions.GetShortPlayerInfo(UsersLastTags[message.Chat.Id], Program.TrackedClans),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: PlayerFunctions.GetFullPlayerInfo(UsersLastTags[message.Chat.Id], Program.TrackedClans),
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
                return;
            }
            if (message.Text == keywords[5])
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
            if (TagRegex.IsMatch(message.Text))
            {
                await CheckClanExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: ClanFunctions.GetClanShortInfo(UsersLastTags[message.Chat.Id], Program.TrackedClans),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                 text: ClanFunctions.GetClanMembers(UsersLastTags[message.Chat.Id], Program.TrackedClans),
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
                  text: ClanFunctions.GetClanSiegeMachines(UsersLastTags[message.Chat.Id], Program.TrackedClans),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[5])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: ClanFunctions.GetClanActiveeSuperUnits(UsersLastTags[message.Chat.Id], Program.TrackedClans),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[6])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                text: ClanFunctions.GetMonthStatistcs(UsersLastTags[message.Chat.Id], Program.TrackedClans),
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

    public async static Task HandleCurrentWarInfoLvl2(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckClanTagMessageExist(botClient, message);

        try
        {
            if (TagRegex.IsMatch(message.Text))
            {
                await CheckClanExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                 text: CurrentStatisticsFunctions.GetCurrentWarShortInfo(UsersLastTags[message.Chat.Id], Program.TrackedClans),
                 parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[1])
            {
                var answer = ClanFunctions.GetWarHistory(UsersLastTags[message.Chat.Id], Program.TrackedClans, 1, messageSplitToken);

                var newAnswers = answer.Split(new[] { messageSplitToken }, StringSplitOptions.None);

                foreach (var answ in newAnswers)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: answ,
                      parseMode: ParseMode.MarkdownV2);
                }

                return;
            }
            if (message.Text == keywords[2])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                 text: CurrentStatisticsFunctions.GetCurrentWarMap(UsersLastTags[message.Chat.Id], Program.TrackedClans),
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

    public async static Task HandleCurrentRaidInfoLvl2(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckClanTagMessageExist(botClient, message);

        try
        {
            if (TagRegex.IsMatch(message.Text))
            {
                await CheckClanExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: CurrentStatisticsFunctions.GetCurrentRaidShortInfo(UsersLastTags[message.Chat.Id], Program.TrackedClans),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[1])
            {
                var answer = ClanFunctions.GetRaidsHistory(UsersLastTags[message.Chat.Id], Program.TrackedClans, 1, messageSplitToken);

                var newAnswers = answer.Split(new[] { messageSplitToken }, StringSplitOptions.None);

                foreach (var answ in newAnswers)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: answ,
                      parseMode: ParseMode.MarkdownV2);
                }

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


    public async static Task HandlePlayerWarStatisticsLvl3(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckMemberTagMessageExist(botClient, message);

        try
        {
            if (TagRegex.IsMatch(message.Text))
            {
                await CheckMemberExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                var answer = PlayerFunctions.GetWarStatistics(UsersLastTags[message.Chat.Id], Program.TrackedClans, 1, messageSplitToken);

                var newAnswers = answer.Split(new[] { messageSplitToken }, StringSplitOptions.None);

                foreach (var answ in newAnswers)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: answ,
                      parseMode: ParseMode.MarkdownV2);
                }

                return;
            }
            if (message.Text == keywords[1])
            {
                var answer = PlayerFunctions.GetWarStatistics(UsersLastTags[message.Chat.Id], Program.TrackedClans, 3, messageSplitToken);

                var newAnswers = answer.Split(new[] { messageSplitToken }, StringSplitOptions.None);

                foreach (var answ in newAnswers)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: answ,
                      parseMode: ParseMode.MarkdownV2);
                }

                return;
            }
            if (message.Text == keywords[2])
            {
                var answer = PlayerFunctions.GetWarStatistics(UsersLastTags[message.Chat.Id], Program.TrackedClans, 5, messageSplitToken);

                var newAnswers = answer.Split(new[] { messageSplitToken }, StringSplitOptions.None);

                foreach (var answ in newAnswers)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: answ,
                      parseMode: ParseMode.MarkdownV2);
                }

                return; ;
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
            if (TagRegex.IsMatch(message.Text))
            {
                await CheckMemberExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                var answer = PlayerFunctions.GetRaidStatistics(UsersLastTags[message.Chat.Id], Program.TrackedClans, 1, messageSplitToken);

                var newAnswers = answer.Split(new[] { messageSplitToken }, StringSplitOptions.None);

                foreach (var answ in newAnswers)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: answ,
                      parseMode: ParseMode.MarkdownV2);
                }

                return;
            }
            if (message.Text == keywords[1])
            {
                var answer = PlayerFunctions.GetRaidStatistics(UsersLastTags[message.Chat.Id], Program.TrackedClans, 3, messageSplitToken);

                var newAnswers = answer.Split(new[] { messageSplitToken }, StringSplitOptions.None);

                foreach (var answ in newAnswers)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: answ,
                      parseMode: ParseMode.MarkdownV2);
                }

                return;
            }
            if (message.Text == keywords[2])
            {
                var answer = PlayerFunctions.GetRaidStatistics(UsersLastTags[message.Chat.Id], Program.TrackedClans, 5, messageSplitToken);

                var newAnswers = answer.Split(new[] { messageSplitToken }, StringSplitOptions.None);

                foreach (var answ in newAnswers)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: answ,
                      parseMode: ParseMode.MarkdownV2);
                }

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
            if (TagRegex.IsMatch(message.Text))
            {
                await CheckMemberExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: PlayerFunctions.GetMembersArmyInfo(UsersLastTags[message.Chat.Id], Program.TrackedClans, UnitType.Hero),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: PlayerFunctions.GetMembersArmyInfo(UsersLastTags[message.Chat.Id], Program.TrackedClans, UnitType.SiegeMachine),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[2])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: PlayerFunctions.GetMembersArmyInfo(UsersLastTags[message.Chat.Id], Program.TrackedClans, UnitType.SuperUnit),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[3])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: PlayerFunctions.GetMembersArmyInfo(UsersLastTags[message.Chat.Id], Program.TrackedClans, UnitType.Unit),
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
            if (TagRegex.IsMatch(message.Text))
            {
                await CheckClanExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                var answer = ClanFunctions.GetWarHistory(UsersLastTags[message.Chat.Id], Program.TrackedClans, 1, messageSplitToken);

                var newAnswers = answer.Split(new[] { messageSplitToken }, StringSplitOptions.None);

                foreach (var answ in newAnswers)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: answ,
                      parseMode: ParseMode.MarkdownV2);
                }

                return;
            }
            if (message.Text == keywords[1])
            {
                var answer = ClanFunctions.GetWarHistory(UsersLastTags[message.Chat.Id], Program.TrackedClans, 3, messageSplitToken);

                var newAnswers = answer.Split(new[] { messageSplitToken }, StringSplitOptions.None);

                foreach (var answ in newAnswers)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: answ,
                      parseMode: ParseMode.MarkdownV2);
                }

                return;
            }
            if (message.Text == keywords[2])
            {
                var answer = ClanFunctions.GetWarHistory(UsersLastTags[message.Chat.Id], Program.TrackedClans, 5, messageSplitToken);

                var newAnswers = answer.Split(new[] { messageSplitToken }, StringSplitOptions.None);

                foreach (var answ in newAnswers)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: answ,
                      parseMode: ParseMode.MarkdownV2);
                }

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
            if (TagRegex.IsMatch(message.Text))
            {
                await CheckClanExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                var answer = ClanFunctions.GetRaidsHistory(UsersLastTags[message.Chat.Id], Program.TrackedClans, 1, messageSplitToken);

                var newAnswers = answer.Split(new[] { messageSplitToken }, StringSplitOptions.None);

                foreach (var answ in newAnswers)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: answ,
                      parseMode: ParseMode.MarkdownV2);
                }

                return;
            }
            if (message.Text == keywords[1])
            {
                var answer = ClanFunctions.GetRaidsHistory(UsersLastTags[message.Chat.Id], Program.TrackedClans, 3, messageSplitToken);

                var newAnswers = answer.Split(new[] { messageSplitToken }, StringSplitOptions.None);

                foreach (var answ in newAnswers)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: answ,
                      parseMode: ParseMode.MarkdownV2);
                }

                return;
            }
            if (message.Text == keywords[2])
            {
                var answer = ClanFunctions.GetRaidsHistory(UsersLastTags[message.Chat.Id], Program.TrackedClans, 5, messageSplitToken);

                var newAnswers = answer.Split(new[] { messageSplitToken }, StringSplitOptions.None);

                foreach (var answ in newAnswers)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: answ,
                      parseMode: ParseMode.MarkdownV2);
                }

                return;
            }
            if (message.Text == keywords[3])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: ClanFunctions.GetMembersAverageRaidsPerfomance(UsersLastTags[message.Chat.Id], Program.TrackedClans),
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

    public async static Task HandleCurrentDistrictStatistics3(ITelegramBotClient botClient, Message message, string[] keywords)
    {
        await CheckClanTagMessageExist(botClient, message);

        try
        {
            if (TagRegex.IsMatch(message.Text))
            {
                await CheckClanExist(botClient, message);

                return;
            }
            if (message.Text == keywords[0])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: CurrentStatisticsFunctions.GetCDistrictStatistics(UsersLastTags[message.Chat.Id], Program.TrackedClans, DistrictType.Capital_Peak),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[1])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: CurrentStatisticsFunctions.GetCDistrictStatistics(UsersLastTags[message.Chat.Id], Program.TrackedClans, DistrictType.Barbarian_Camp),
                  parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[2])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                    text: CurrentStatisticsFunctions.GetCDistrictStatistics(UsersLastTags[message.Chat.Id], Program.TrackedClans, DistrictType.Wizard_Valley),
                    parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[3])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                    text: CurrentStatisticsFunctions.GetCDistrictStatistics(UsersLastTags[message.Chat.Id], Program.TrackedClans, DistrictType.Balloon_Lagoon),
                    parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[4])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                         text: CurrentStatisticsFunctions.GetCDistrictStatistics(UsersLastTags[message.Chat.Id], Program.TrackedClans, DistrictType.Builders_Workshop),
                         parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[5])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                    text: CurrentStatisticsFunctions.GetCDistrictStatistics(UsersLastTags[message.Chat.Id], Program.TrackedClans, DistrictType.Dragon_Cliffs),
                    parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[6])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                               text: CurrentStatisticsFunctions.GetCDistrictStatistics(UsersLastTags[message.Chat.Id], Program.TrackedClans, DistrictType.Golem_Quarry),
                               parseMode: ParseMode.MarkdownV2);

                return;
            }
            if (message.Text == keywords[7])
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,
                     text: CurrentStatisticsFunctions.GetCDistrictStatistics(UsersLastTags[message.Chat.Id], Program.TrackedClans, DistrictType.Skeleton_Park),
                     parseMode: ParseMode.MarkdownV2);

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
        if (!UsersLastTags.ContainsKey(message.Chat.Id))
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
            text: "Введите тег игрока в формате #123456789, а затем выберите пункт из меню");
        }
    }

    public async static Task CheckClanTagMessageExist(ITelegramBotClient botClient, Message message)
    {
        if (!UsersLastTags.ContainsKey(message.Chat.Id))
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
                var member = clan.ClanMembers.First(x => x.Tag == UsersLastTags[message.Chat.Id]);
                await botClient.SendTextMessageAsync(message.Chat.Id,
                text: $"У меня есть информация об этом игроке. Что бы вы хотели узнать?");
                break;

            }
            catch (Exception)
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