using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot;

public static class Menu
{
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> MainKeyboards { get; } = FirstLevelMenu.MainKeyboard;

    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> MemberKeyboards2 { get; } = SecondLevelMenu.MemberKeyboards;
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> LeaderKeyboards2 { get; } = SecondLevelMenu.LeaderKeyboards;
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> OtherKeyboards2 { get; } = SecondLevelMenu.OtherKyboards;

    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> MemberKeyboards3 { get; } = ThirdLevelMenu.MemberKeyboards;
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> LeaderKeyboards3 { get; } = ThirdLevelMenu.LeaderKeyboards;
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> OtherKeyboards3 { get; } = ThirdLevelMenu.OtherKyboards;

    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> MemberKeyboards4 { get; } = FourthLevelMenu.MemberKeyboards;
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> LeaderKeyboards4 { get; } = FourthLevelMenu.LeaderKeyboards;
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> OtherKeyboards4 { get; } = FourthLevelMenu.OtherKyboards;

    public static List<string> Lvl1MainWords { get; set; } = new List<string>()
    {
    "Член клана", "Руководитель", "Прочее"
    };

    public static List<string> Lvl2MemberWords { get; set; } = new List<string>()
    {
    "Игрок", "Клан",
    "Текущая война", "Текущий рейд",
    "Текущий розыгрыш", "Назад"
    };
    public static List<string> Lvl2LeaderWords { get; set; } = new List<string>()
    {
    "Удалить текущую войну", "Добавить текущую войну",
    "Удалить текущий рейд", "Добавить текущий рейд" ,
    "Удалить отслеживаемый клан", "Добавить отслеживаемы",
    "Удалить текущий розыгрыш", "Добавить розыгрыш",
    "Обновить информацию об игроках", "Назад",
    };
    public static List<string> Lvl2OtherWords { get; set; } = new List<string>()
    {
    "Советы по использованию бота", "Правила клана",
    "Ссылки на кланы и ТГ", "Полезные ресурсы по CoC",
    "Оставить отзыв", "Назад"
    };

    public static List<string> Lvl3PlayerInfoWords { get; set; } = new List<string>()
    {
    "Главное об игроке", "Все об игроке",
    "Показатели войн", "Показатели рейдов" ,
    "Розыгрыш", "Войска",
    "История кармы", "Назад"
    };
    public static List<string> Lvl3ClanInfoWords { get; set; } = new List<string>()
    {
    "Главное о клане", "Члены клана" ,
    "История войн", "История рейдов",
    "Осадные машины", "Активные супер юниты",
    "История розыгрышей", "Назад"
    };
    public static List<string> Lvl3CurrentWarInfoWords { get; set; } = new List<string>()
    {
    "Главное", "Показатели",
    "Карта", "Назад",
    };
    public static List<string> Lvl3CurrentRaidInfoWords { get; set; } = new List<string>()
    {
    "Главное", "Показатели",
    "Средние показатели", "Статистика по районам",
    "Назад",
    };
    public static List<string> Lvl3CurrentPrizeDrawInfoWords { get; set; } = new List<string>()
    {
    "Главное", "Показатели",
    "Описание", "Назад",
    };
    public static List<string> Lvl3LeaderChangeConfirmationWords { get; set; } = new List<string>()
    {
    "Отмена", "Применить изменения",
    "Назад",
    };

    public static List<string> Lvl4WarStatisticsWords { get; set; } = new List<string>()
    {
    "Последняя война", "Последние 3",
    "Последние 5", "Назад",
    };
    public static List<string> Lvl4WarHistoryWords { get; set; } = new List<string>()
    {
    "Последняя война", "Последние 3",
    "Последние 5", "Назад",
    };
    public static List<string> Lvl4RaidStatisticsWords { get; set; } = new List<string>()
    {
    "Последний рейд", "Последние 3",
    "Последние 5", "Назад",
    };
    public static List<string> Lvl4RaidHistoryWords { get; set; } = new List<string>()
    {
    "Последний рейд", "Последние 3",
    "Последние 5", "Назад",
    };
    public static List<string> Lvl4ArmyWords { get; set; } = new List<string>()
    {
    "Герои игрока", "Осадные машины игрока",
    "Супер юниты игрока", "Все войска игрока",
    "Назад",
    };
    public static List<string> Lvl4PrizeDrawHistoryWords { get; set; } = new List<string>()
    {
    "Главное", "Показатели",
    "Описание", "Назад",
    };
    public static List<string> Lvl4DistrictsStatisticsWords { get; set; } = new List<string>()
    {
     "Столичный пик", "Лагерь варваров", "Долина колдунов",
    "Лагуна шаров", "Мастерская строителя", "Драконьи утесы",
    "Карьер големов", "Парк скелетов", "Назад",
    };
}

public enum MenuLevel
{
    Main,

    Member2,
    Member3,
    Member4,

    Leader2,
    Leader3,
    Leader4,

    Other2,
    Other3,
    Other4
}

public enum KeyboardType
{
    MainMenu,

    Member,
    Leader,
    OtherInfo,

    PlayerInfo,
    ClanInfo,
    CurrentWarInfo,
    CurrentRaidInfo,
    CurrentPrizedrawInfo,
    LeaderChangeConfirmation,

    PlayerWarStatistics,
    PlayerRaidStatistics,
    PlayerArmy,
    ClanWarsHistory,
    ClanRaidsHistory,
    ClanPrizeDrawHistory,
    RaidDistrictsStatistics
}
