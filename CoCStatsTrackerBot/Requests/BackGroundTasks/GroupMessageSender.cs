using CoCStatsTracker;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot.Requests;

public static class GroupMessageSender
{
    public static void SendMessageToClanGroups(ITelegramBotClient botClient)
    {
        var trackedClans = GetFromDbQueryHandler.GetAllTrackedClans();

        foreach (var trackedClan in trackedClans.Where(x => x.ClansTelegramChatId != null && x.RegularNewsLetterOn == true))
        {
            try
            {
               var abc = botClient.SendTextMessageAsync(trackedClan.ClansTelegramChatId,
                           text: "Проверка связи",
                           parseMode: ParseMode.MarkdownV2);
            }
            catch (Exception e)
            {
                continue;
            }
        }
        
    }
}