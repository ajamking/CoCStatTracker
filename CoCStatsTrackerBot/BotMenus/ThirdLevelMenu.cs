using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot;

public class ThirdLevelMenu
{
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> MemberKeyboards { get; } = new Dictionary<KeyboardType, ReplyKeyboardMarkup>();
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> LeaderKeyboards { get; } = new Dictionary<KeyboardType, ReplyKeyboardMarkup>();
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> OtherKyboards { get; } = new Dictionary<KeyboardType, ReplyKeyboardMarkup>(); //Пока пусто

    static ThirdLevelMenu()
    {
        ReplyKeyboardMarkup playerInfo = new(new[]
              {
             new KeyboardButton[] { "Главное об игроке", "Все об игроке" },
             new KeyboardButton[] { "Показатели войн", "Показатели рейдов" },
             new KeyboardButton[] { "Розыгрыш", "Войска" },
             new KeyboardButton[] { "История кармы", "Назад" }
       })
        {
            ResizeKeyboard = true
        };

        ReplyKeyboardMarkup clanInfo = new(new[]
              {
             new KeyboardButton[] { "Главное о клане", "Члены клана" },
             new KeyboardButton[] { "История войн", "История рейдов" },
             new KeyboardButton[] { "Осадные машины", "Активные супер юниты" },
             new KeyboardButton[] { "История розыгрышей", "Назад" }
       })
        {
            ResizeKeyboard = true
        };

        ReplyKeyboardMarkup currentWarInfo = new(new[]
              {
             new KeyboardButton[] { "Главное", "Показатели" },
             new KeyboardButton[] { "Карта", "Назад" },
       })
        {
            ResizeKeyboard = true
        };

        ReplyKeyboardMarkup currentRaidInfo = new(new[]
             {
             new KeyboardButton[] { "Главное", "Показатели" },
             new KeyboardButton[] { "Средние показатели", "Статистика по районам" },
              new KeyboardButton[] { "Назад" },
       })
        {
            ResizeKeyboard = true
        };

        ReplyKeyboardMarkup currentPrizeDrawInfo = new(new[]
             {
             new KeyboardButton[] { "Главное", "Показатели" },
             new KeyboardButton[] { "Описание", "Назад" },
       })
        {
            ResizeKeyboard = true
        };

        MemberKeyboards.Add(KeyboardType.PlayerInfo, playerInfo);
        MemberKeyboards.Add(KeyboardType.ClanInfo, clanInfo);
        MemberKeyboards.Add(KeyboardType.CurrentWarInfo, currentWarInfo);
        MemberKeyboards.Add(KeyboardType.CurrentRaidInfo, currentRaidInfo);
        MemberKeyboards.Add(KeyboardType.CurrentPrizedrawInfo, currentPrizeDrawInfo);

        ReplyKeyboardMarkup leaderChangeConfirmation = new(new[]
             {
             new KeyboardButton[] { "Отмена", "Применить изменения" },
             new KeyboardButton[] { "Назад" },
       })
        {
            ResizeKeyboard = true
        };

        LeaderKeyboards.Add(KeyboardType.LeaderChangeConfirmation, leaderChangeConfirmation);
    }
    
}
