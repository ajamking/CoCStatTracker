using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot.Menue;

public class Menues
{
    public List<BaseMenu> AllMenues { get; set; } = new List<BaseMenu>() { new MainMenu0() };

    public Menues()
    {
        AllMenues.Add(new MemberMenu1());
        AllMenues.Add(new PlayerInfo2());
        AllMenues.Add(new ClanInfo2());
        AllMenues.Add(new CurrentWarInfo2());
        AllMenues.Add(new CurrentRaidInfo2());
        AllMenues.Add(new PlayerWarStatistics3());
        AllMenues.Add(new PlayerRaidStatistics3());
        AllMenues.Add(new PlayerArmy3());
        AllMenues.Add(new ClanWarHistory3());
        AllMenues.Add(new ClanRaidHistory3());
        AllMenues.Add(new CurrentDistrictStatistics3());
    }
}

public class BaseMenu
{
    public string Header { get; init; }
    public string[] KeyWords { get; init; }
    public ReplyKeyboardMarkup Keyboard { get; init; }
    public MenuLevels MenuLevel { get; init; }
}

public class MainMenu0 : BaseMenu
{
    public MainMenu0()
    {
        Header = "/start";

        KeyWords = new string[] { "Член клана", "Руководитель", "Прочее" };

        Keyboard = new(new[]
        {
        new KeyboardButton[] { KeyWords[0], KeyWords[1], KeyWords[2] },
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevels.Main0;
    }
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