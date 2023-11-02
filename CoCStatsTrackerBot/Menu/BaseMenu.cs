using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot.Menu;

public class BaseMenu
{
    public string Header { get; init; }
    public string[] KeyWords { get; init; }
    public ReplyKeyboardMarkup Keyboard { get; init; }
    public MenuLevel MenuLevel { get; init; }
}

public enum MenuLevel
{
    Main0,

    Member1,

    PlayerInfo2,
    ClanInfo2,
    CurrentWarInfo2,
    CurrentRaidInfo2,

    PlayerWarStatistics3,
    PlayerRaidStatistics3,
    PlayerArmy3,

    ClanWarsHistory3,
    ClanRaidsHistory3,

    CurrentDistrictStatistics3
}