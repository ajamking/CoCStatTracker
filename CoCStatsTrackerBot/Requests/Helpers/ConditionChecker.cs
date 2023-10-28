using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace CoCStatsTrackerBot.Requests;

public static class ConditionChecker
{
    //public async static Task CheckMemberTagMessageExist(ITelegramBotClient botClient, Message message)
    //{
    //    if (!Navigator.UsersLastTags.ContainsKey(message.Chat.Id))
    //    {
    //        await botClient.SendTextMessageAsync(message.Chat.Id,
    //        text: "Введите тег игрока в формате #123456789, а затем выберите пункт из меню");
    //    }
    //}

    //public async static Task<bool> CheckClanTagMessageExist(ITelegramBotClient botClient, Message message)
    //{
    //    if (!Navigator.UsersLastTags.ContainsKey(message.Chat.Id))
    //    {
    //        var str = new StringBuilder("Введите ТЕГ одного из тегов отслеживаемых кланов, а затем выберите пункт из меню:\n");

    //        foreach (var clan in Program.TrackedClans)
    //        {
    //            if (clan.IsCurrent)
    //            {
    //                str.AppendLine(StylingHelper.MakeItStyled(clan.Name + " - " + clan.Tag, UiTextStyle.Name));
    //            }
    //        }

    //        await botClient.SendTextMessageAsync(message.Chat.Id,
    //        text: str.ToString(),
    //        parseMode: ParseMode.MarkdownV2);
    //    }

    //    return true;
    //}

    //public async static Task CheckMemberExist(ITelegramBotClient botClient, Message message)
    //{
    //    foreach (var clan in Program.TrackedClans.Where(x => x.IsCurrent == true))
    //    {
    //        try
    //        {
    //            var member = clan.ClanMembers.First(x => x.Tag == Navigator.UsersLastTags[message.Chat.Id]);
    //            await botClient.SendTextMessageAsync(message.Chat.Id,
    //            text: $"У меня есть информация об этом игроке. Что бы вы хотели узнать?");
    //            break;

    //        }
    //        catch (Exception)
    //        {
    //            await botClient.SendTextMessageAsync(message.Chat.Id,
    //            text: $"Игрока с таки тегом нет в отслеживаемых кланах, попробуйте ввести тег повторно");
    //            break;
    //        }
    //    }
    //}

    //public async static Task CheckClanExist(ITelegramBotClient botClient, Message message)
    //{
    //    if (Program.TrackedClans.Where(x => x.IsCurrent == true).Any(x => x.Tag == message.Text))
    //    {
    //        await botClient.SendTextMessageAsync(message.Chat.Id,
    //                text: $"У меня есть информация об этом клане. Что бы вы хотели узнать?");
    //    }
    //    else
    //    {
    //        var str = new StringBuilder("Я не отслеживаю клан с таким тегом, попробуйте ввести тег одного из отслеживаемых кланов:\n");

    //        foreach (var clan in Program.TrackedClans)
    //        {
    //            if (clan.IsCurrent)
    //            {
    //                str.AppendLine(StylingHelper.MakeItStyled(clan.Name + " - " + clan.Tag, UiTextStyle.Name));
    //            }
    //        }

    //        await botClient.SendTextMessageAsync(message.Chat.Id,
    //        text: str.ToString(),
    //        parseMode: ParseMode.MarkdownV2);
    //    }
    //}

}
