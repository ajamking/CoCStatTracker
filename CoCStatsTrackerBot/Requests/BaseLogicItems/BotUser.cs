using CoCStatsTrackerBot.Menu;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CoCStatsTrackerBot.Requests;

public class BotUser
{
    public long ChatId { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public MenuLevel CurrentMenuLevel { get; set; }
    public MenuLevel PreviousMenuLevel { get; set; }

    public bool IsBotHolder { get; set; }
    public bool IsAdmin { get; set; }
    public string AdminsKey { get; set; }


    public RequestHadnlerParameters RequestHadnlerParameters { get; set; }

    public BotUser(ITelegramBotClient botClient, Message message)
    {
        ChatId = message.Chat.Id;
        Username = message.Chat.Username;
        FirstName = message.Chat.FirstName;
        CurrentMenuLevel = MenuLevel.Main0;
        IsAdmin = false;
        RequestHadnlerParameters = new RequestHadnlerParameters(botClient, message, null, null);
    }
}