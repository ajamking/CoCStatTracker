using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot;

public class FourthLevelMenu
{
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> MemberKeyboards { get; } = new Dictionary<KeyboardType, ReplyKeyboardMarkup>();
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> LeaderKeyboards { get; } = new Dictionary<KeyboardType, ReplyKeyboardMarkup>(); //Пока пусто
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> OtherKyboards { get; } = new Dictionary<KeyboardType, ReplyKeyboardMarkup>(); //Пока пусто

    static FourthLevelMenu()
    {
        ReplyKeyboardMarkup warStatistics = new(new[]
              {
             new KeyboardButton[] { "Последняя война", "Последние 3" },
             new KeyboardButton[] { "Последние 5", "Назад" },
        })
        {
            ResizeKeyboard = true
        };

        ReplyKeyboardMarkup warHistory = new(new[]
             {
             new KeyboardButton[] { "Последняя война", "Последние 3" },
             new KeyboardButton[] { "Последние 5", "Назад" },
        })
        {
            ResizeKeyboard = true
        };

        ReplyKeyboardMarkup raidStatistics = new(new[]
              {
             new KeyboardButton[] { "Последний рейд", "Последние 3" },
             new KeyboardButton[] { "Последние 5", "Назад" },
        })
        {
            ResizeKeyboard = true
        };

        ReplyKeyboardMarkup raidsHistory = new(new[]
             {
             new KeyboardButton[] { "Последний рейд", "Последние 3" },
             new KeyboardButton[] { "Последние 5", "Назад" },
        })
        {
            ResizeKeyboard = true
        };

        ReplyKeyboardMarkup army = new(new[]
            {
             new KeyboardButton[] { "Герои", "Осадные машины" },
             new KeyboardButton[] { "Супер юниты", "Все юниты" },
             new KeyboardButton[] { "Назад" },
        })
        {
            ResizeKeyboard = true
        };

        ReplyKeyboardMarkup prizeDrawHistory = new(new[]
             {
             new KeyboardButton[] { "Главное", "Показатели" },
             new KeyboardButton[] { "Описание", "Назад" },
        })
        {
            ResizeKeyboard = true
        };

        ReplyKeyboardMarkup districtsStatistics = new(new[]
              {
             new KeyboardButton[] { "Столичный пик", "Лагерь варваров", "Долина колдунов" },
             new KeyboardButton[] { "Лагуна шаров", "Мастерская строителя", "Драконьи утесы" },
             new KeyboardButton[] { "Карьер големов", "Парк скелетов", "Назад" },
        })
        {
            ResizeKeyboard = true
        };

        MemberKeyboards.Add(KeyboardType.PlayerWarStatistics, warStatistics);
        MemberKeyboards.Add(KeyboardType.ClanWarsHistory, warHistory);
        MemberKeyboards.Add(KeyboardType.PlayerRaidStatistics, raidStatistics);
        MemberKeyboards.Add(KeyboardType.ClanRaidsHistory, raidsHistory);
        MemberKeyboards.Add(KeyboardType.PlayerArmy, army);
        MemberKeyboards.Add(KeyboardType.ClanPrizeDrawHistory, prizeDrawHistory);
        MemberKeyboards.Add(KeyboardType.RaidDistrictsStatistics, districtsStatistics);
    }
}
