using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot.Menu;

public class MainMenu0 : BaseMenu
{
    public MainMenu0()
    {
        Header = "/start";

        KeyWords = new string[] 
        { 
            "Член клана", "Руководитель", "Прочее"
        };

        Keyboard = new(new[]
        {
        new KeyboardButton[] { KeyWords[0], KeyWords[1], KeyWords[2] },
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.Main0;
    }

}

public class MemberMenu1 : BaseMenu
{
    public MemberMenu1()
    {
        Header = "Член клана";

        KeyWords = new string[]
        {
            "Игрок", "Клан",
            "Текущая война", "Текущий рейд",
            "Назад"
        };

        Keyboard = new(new[]
        {
        new KeyboardButton[] { KeyWords[0], KeyWords[1] },
        new KeyboardButton[] { KeyWords[2], KeyWords[3] },
        new KeyboardButton[] { KeyWords[4] }
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.Member1;
    }
}

public class PlayerInfo2 : BaseMenu
{
    public PlayerInfo2()
    {
        Header = "Игрок";

        KeyWords = new string[]
        {
            "Главное об игроке", "Все об игроке",
            "Показатели войн", "Показатели рейдов",
            "Войска", "Назад"
        };

        Keyboard = new(new[]
        {
        new KeyboardButton[] { KeyWords[0], KeyWords[1] },
        new KeyboardButton[] { KeyWords[2], KeyWords[3] },
        new KeyboardButton[] { KeyWords[4], KeyWords[5] },
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.PlayerInfo2;
    }
}

public class ClanInfo2 : BaseMenu
{
    public ClanInfo2()
    {
        Header = "Клан";

        KeyWords = new string[]
        {
            "Главное о клане", "Члены клана",
            "История войн", "История рейдов",
            "Осадные машины", "Активные супер юниты",
            "Показатели месяца", "Назад"
        };

        Keyboard = new(new[]
        {
        new KeyboardButton[] { KeyWords[0], KeyWords[1] },
        new KeyboardButton[] { KeyWords[2], KeyWords[3] },
        new KeyboardButton[] { KeyWords[4], KeyWords[5] },
        new KeyboardButton[] { KeyWords[6], KeyWords[7] }
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.ClanInfo2;
    }
}

public class ClanCurrentRaidInfo2 : BaseMenu
{
    public ClanCurrentRaidInfo2()
    {
        Header = "Текущий рейд";

        KeyWords = new string[]
        {
            "Главное о рейде", "Показатели рейда",
            "Статистика по районам", "Назад",
        };

        Keyboard = new(new[]
        {
        new KeyboardButton[] { KeyWords[0], KeyWords[1] },
        new KeyboardButton[] { KeyWords[2], KeyWords[3] },
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.CurrentRaidInfo2;
    }
}

public class ClanCurrentWarInfo2 : BaseMenu
{
    public ClanCurrentWarInfo2()
    {
        Header = "Текущая война";

        KeyWords = new string[]
        {
           "Главное о войне", "Показатели войны",
           "Карта", "Назад",
        };

        Keyboard = new(new[]
        {
        new KeyboardButton[] { KeyWords[0], KeyWords[1] },
        new KeyboardButton[] { KeyWords[2], KeyWords[3] },
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.CurrentWarInfo2;
    }
}

public class PlayerWarStatistics3 : BaseMenu
{
    public PlayerWarStatistics3()
    {
        Header = "Показатели войн";

        KeyWords = new string[]
        {
           "Последняя участие в войне", "3 последних войны",
           "5 последних войн", "Назад",
        };

        Keyboard = new(new[]
        {
        new KeyboardButton[] { KeyWords[0], KeyWords[1] },
        new KeyboardButton[] { KeyWords[2], KeyWords[3] },
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.PlayerWarStatistics3;
    }
}

public class PlayerRaidStatistics3 : BaseMenu
{
    public PlayerRaidStatistics3()
    {
        Header = "Показатели рейдов";

        KeyWords = new string[]
        {
            "Последнее участие в рейдах", "3 последних рейда",
            "5 последних рейдов", "Назад",
        };

        Keyboard = new(new[]
        {
        new KeyboardButton[] { KeyWords[0], KeyWords[1] },
        new KeyboardButton[] { KeyWords[2], KeyWords[3] },
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.PlayerRaidStatistics3;
    }
}

public class PlayerArmy3 : BaseMenu
{
    public PlayerArmy3()
    {
        Header = "Войска";

        KeyWords = new string[]
        {
             "Герои", "Осадные машины игрока",
             "Активные супер юниты игрока", "Все войска",
             "Назад",
        };

        Keyboard = new(new[]
        {
        new KeyboardButton[] { KeyWords[0], KeyWords[1] },
        new KeyboardButton[] { KeyWords[2], KeyWords[3] },
        new KeyboardButton[] { KeyWords[4] },
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.PlayerArmy3;
    }
}

public class ClanWarHistory3 : BaseMenu
{
    public ClanWarHistory3()
    {
        Header = "История войн";

        KeyWords = new string[]
        {
           "Последняя война", "Последние 3",
           "Последние 5", "Назад",
        };

        Keyboard = new(new[]
        {
        new KeyboardButton[] { KeyWords[0], KeyWords[1] },
        new KeyboardButton[] { KeyWords[2], KeyWords[3] },
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.ClanWarsHistory3;
    }
}

public class ClanRaidHistory3 : BaseMenu
{
    public ClanRaidHistory3()
    {
        Header = "История рейдов";

        KeyWords = new string[]
        {
           "Последний рейд", "Последние_3",
           "Последние_5", "Средние показатели игроков",
           "Назад",
        };

        Keyboard = new(new[]
        {
        new KeyboardButton[] { KeyWords[0], KeyWords[1] },
        new KeyboardButton[] { KeyWords[2], KeyWords[3] },
        new KeyboardButton[] { KeyWords[4]},
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.ClanRaidsHistory3;
    }
}

public class ClanCurrentDistrictStatistics3 : BaseMenu
{
    public ClanCurrentDistrictStatistics3()
    {
        Header = "Статистика по районам";

        KeyWords = new string[]
        {
           "Столичный пик", "Лагерь варваров", "Долина колдунов",
           "Лагуна шаров", "Мастерская строителя", "Драконьи утесы",
           "Карьер големов", "Парк скелетов", "Назад",
        };

        Keyboard = new(new[]
        {
          new KeyboardButton[] { KeyWords[0], KeyWords[1], KeyWords[2] },
          new KeyboardButton[] { KeyWords[3], KeyWords[4], KeyWords[5] },
          new KeyboardButton[] { KeyWords[6], KeyWords[7], KeyWords[8] },
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.CurrentDistrictStatistics3;
    }
}