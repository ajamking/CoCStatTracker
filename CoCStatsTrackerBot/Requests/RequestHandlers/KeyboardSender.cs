using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot.Requests;

public static class KeyboardSender
{
    public async static void ShowKeyboard(RequestHadnlerParameters parameters, ReplyKeyboardMarkup keyboard)
    {
        await parameters.BotClient.SendTextMessageAsync(parameters.Message.Chat.Id,
                              text: "Выберите интересующий пункт из меню",
                              replyMarkup: keyboard);
    }
}
