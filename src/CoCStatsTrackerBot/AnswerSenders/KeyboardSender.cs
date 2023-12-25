using CoCStatsTrackerBot.Requests;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot;

public static class KeyboardSender
{
    public async static void ShowKeyboard(BotUserRequestParameters parameters, ReplyKeyboardMarkup keyboard)
    {
		try
		{
            await parameters.BotClient.SendTextMessageAsync(parameters.Message.Chat.Id,
                             text: "Выберите интересующий пункт из меню",
                             replyMarkup: keyboard);
        }
		catch
		{
            return;
		}
    }
}