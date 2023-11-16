using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot.BotMenues;

public class MainMenu0 : BaseMenu
{
    public MainMenu0()
    {
        Header = "/start";

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Основные функции", "Интерфейс главы клана", "Прочее" },
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.Main0;
    }

}


public class MemberMenu1 : BaseMenu
{
    public MemberMenu1()
    {
        Header = "Основные функции";

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Игрок", "Клан" },
        new KeyboardButton[] { "Текущая война", "Текущий рейд" },
        new KeyboardButton[] { "Все отслеживаемые кланы", "Назад" }
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

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Главное об игроке", "Все об игроке" },
        new KeyboardButton[] { "Показатели войн", "Показатели рейдов" },
        new KeyboardButton[] { "Войска", "Назад" },
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

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Главное о клане", "Члены клана" },
        new KeyboardButton[] { "История войн","История рейдов" },
        new KeyboardButton[] { "Осадные машины","Активные супер юниты" },
        new KeyboardButton[] { "Показатели месяца", "Назад" }
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

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Главное о рейде", "Показатели рейда" },
        new KeyboardButton[] { "Статистика по районам", "Назад" },
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

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Главное о войне", "Показатели войны" },
        new KeyboardButton[] { "Карта", "Назад" },
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

        Keyboard = new(new[]
        {
        new KeyboardButton[] {  "Последнее участие в войне", "3 последних войны" },
        new KeyboardButton[] {  "5 последних войн", "Назад" },
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

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Последнее участие в рейдах", "3 последних рейда" },
        new KeyboardButton[] { "5 последних рейдов", "Назад" },
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

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Герои", "Осадные машины игрока" },
        new KeyboardButton[] { "Активные супер юниты игрока", "Обычные войска" },
        new KeyboardButton[] { "Назад" },
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

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Последняя война", "Последние 3" },
        new KeyboardButton[] { "Последние 5", "Последние 10" },
        new KeyboardButton[] { "Назад" },
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

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Последний рейд", "Последние_3" },
        new KeyboardButton[] { "Последние_5", "Последние_10" },
        new KeyboardButton[] { "Средние показатели игроков", "Назад" },
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

        Keyboard = new(new[]
        {
          new KeyboardButton[] { "Столичный пик", "Лагерь варваров", "Долина колдунов" },
          new KeyboardButton[] { "Лагуна шаров", "Мастерская строителя", "Драконьи утесы" },
          new KeyboardButton[] { "Карьер големов", "Парк скелетов", "Гоблинские шахты"},
          new KeyboardButton[] { "Назад"}
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.CurrentDistrictStatistics3;
    }
}


public class LeaderMenu1 : BaseMenu
{
    public LeaderMenu1()
    {
        Header = "Интерфейс главы клана";

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Доступные кланы" },
        new KeyboardButton[] { "Добавление", "Обновление" },
        new KeyboardButton[] { "Удаление", "Настройки ТГ группы" },
        new KeyboardButton[] { "Меню создателя", "Назад" }
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.Leader1;
    }
}

public class DeveloperMenu2 : BaseMenu
{
    public DeveloperMenu2()
    {
        Header = "Меню создателя";

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Главное окно разработчика", "Установить клану токен" },
        new KeyboardButton[] { "Добавить клан в БД", "Удалить клан из БД", },
        new KeyboardButton[] { "Добавить клан в ЧС", "Удалить клан из ЧС" },
        new KeyboardButton[] { "Установить клану ChatId.", "Назад" }
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.DeveloperMenu2;
    }
}

public class LeaderAddMenu2 : BaseMenu
{
    public LeaderAddMenu2()
    {
        Header = "Добавление";

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Добавить последний рейд", "Добавить последнюю войну" },
        new KeyboardButton[] { "Назад" }
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.LeaderAddMenu2;
    }
}

public class LeaderUpdateMenu2 : BaseMenu
{
    public LeaderUpdateMenu2()
    {
        Header = "Обновление";

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Описание функций",},
        new KeyboardButton[] { "Характеристики клана", "Игроков клана",},
        new KeyboardButton[] { "Обновить последний рейд", "Обновить последнюю войну"},
        new KeyboardButton[] { "Сбросить сезонные показатели", "Назад" }
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.LeaderUpdateMenu2;
    }
}

public class LeaderDeleteMenu2 : BaseMenu
{
    public LeaderDeleteMenu2()
    {
        Header = "Удаление";

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Удаление войн", "Удаление рейдов" },
        new KeyboardButton[] { "Назад" }
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.LeaderDeleteMenu2;
    }
}

public class TgGroupCustomize : BaseMenu
{
    public TgGroupCustomize()
    {
        Header = "Настройки ТГ группы";

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Руководство", "Список членов клана" },
        new KeyboardButton[] { "Включить рассылку", "Выключить рассылку" },
        new KeyboardButton[] { "Установить клану ChatId", "Назад" }
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.LeaderTgGroupCustomize2;
    }
}

public class LeaderDeleteRaidsMenu3 : BaseMenu
{
    public LeaderDeleteRaidsMenu3()
    {
        Header = "Удаление рейдов";

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Зафиксированные рейды.",  "Оставить последний." },
        new KeyboardButton[] { "Оставить 3 последних.", "Оставить 5 последних."},
        new KeyboardButton[] { "Удалить все.", "Назад" }
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.LeaderDeleteRaidsMenu3;
    }
}

public class LeaderDeleteClanWarsMenu3 : BaseMenu
{
    public LeaderDeleteClanWarsMenu3()
    {
        Header = "Удаление войн";

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Зафиксированные войны", "Оставить последнюю" },
        new KeyboardButton[] { "Оставить 3 последних", "Оставить 5 последних" },
        new KeyboardButton[] { "Удалить все", "Назад" }
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.LeaderDeleteClanWarsMenu3;
    }
}

public class OtherMenu1 : BaseMenu
{
    public OtherMenu1()
    {
        Header = "Прочее";

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Дефолтные планировки столицы", "Полезные ссылки" },
        new KeyboardButton[] { "Контакты", "Оставить отзыв" },
        new KeyboardButton[] { "CoC Стикеры", "Назад" }
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.Other1;
    }
}

public class Layouts2 : BaseMenu
{
    public Layouts2()
    {
        Header = "Планировки";

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Основная деревня", "Деревня строителя" },
        new KeyboardButton[] { "Столица кланов", "Назад" },
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.Layouts2;
    }
}

public class ThLayouts3 : BaseMenu
{
    public ThLayouts3()
    {
        Header = "Планировки";

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Тх 11", "Тх 12" },
        new KeyboardButton[] { "Тх 13", "Тх 14" },
        new KeyboardButton[] { "Тх 15", "Назад" },
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.ThLayouts3;
    }
}

public class BbLayouts3 : BaseMenu
{
    public BbLayouts3()
    {
        Header = "Планировки";

        Keyboard = new(new[]
        {
        new KeyboardButton[] { "Дс 8", "Дс 9" },
        new KeyboardButton[] { "Дс 10", "Назад" },
        })
        { ResizeKeyboard = true };

        MenuLevel = MenuLevel.BbLayouts3;
    }
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
    CurrentDistrictStatistics3,

    Leader1,

    LeaderAddMenu2,
    LeaderUpdateMenu2,
    LeaderDeleteMenu2,
    LeaderTgGroupCustomize2,
    DeveloperMenu2,

    LeaderDeleteClanWarsMenu3,
    LeaderDeleteRaidsMenu3,

    Other1,

    Layouts2,

    ThLayouts3,
    BbLayouts3,
}