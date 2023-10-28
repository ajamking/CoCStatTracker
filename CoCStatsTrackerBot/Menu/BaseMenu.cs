using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot.Menu;

public static class Menu
{
    public static List<BaseMenu> AllMenues { get; set; } = new List<BaseMenu>() { new MainMenu0() };

    static Menu()
    {
        AllMenues.Add(new MemberMenu1());
        AllMenues.Add(new PlayerInfo2());
        AllMenues.Add(new ClanInfo2());
        AllMenues.Add(new ClanCurrentWarInfo2());
        AllMenues.Add(new ClanCurrentRaidInfo2());
        AllMenues.Add(new PlayerWarStatistics3());
        AllMenues.Add(new PlayerRaidStatistics3());
        AllMenues.Add(new PlayerArmy3());
        AllMenues.Add(new ClanWarHistory3());
        AllMenues.Add(new ClanRaidHistory3());
        AllMenues.Add(new ClanCurrentDistrictStatistics3());
    }
}

public class BaseMenu
{
    public string Header { get; init; }
    public string[] KeyWords { get; init; }
    public ReplyKeyboardMarkup Keyboard { get; init; }
    public MenuLevels MenuLevel { get; init; }
}

public enum MenuLevels
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