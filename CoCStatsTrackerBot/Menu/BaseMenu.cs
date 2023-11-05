using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot.Menu;

public class BaseMenu
{
    public string Header { get; init; }
    public ReplyKeyboardMarkup Keyboard { get; init; }
    public MenuLevel MenuLevel { get; init; }
}