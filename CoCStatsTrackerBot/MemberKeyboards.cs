using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot;

public static class MemberKeyboards
{
    public static Dictionary<string, ReplyKeyboardMarkup> CustomKeyboards { get; } = new Dictionary<string, ReplyKeyboardMarkup>();

    static MemberKeyboards()
    {
        ReplyKeyboardMarkup mainKeyboard = new(new[]
       {
             new KeyboardButton[] { "Игрок", "Клан" },
             new KeyboardButton[] { "Текущая война", "Текущий рейд" },
             new KeyboardButton[] { "Розыгрыш", "Выбор интерфейса"}
       })
        {
            ResizeKeyboard = true
        };

        ReplyKeyboardMarkup playerKeyboard =
        new(new[]
        {
                new KeyboardButton[] { "Все об игроке", "Главное об игроке" },
                new KeyboardButton[] { "Показатели войн", "Показатели рейдов" },
                new KeyboardButton[] { "Показатели розыгрыша", "Войска" },
                new KeyboardButton[] { "История кармы", "В главное меню"},
        })
        {
            ResizeKeyboard = true
        };

        ReplyKeyboardMarkup clanKeyboard =
       new(new[]
       {
                new KeyboardButton[] { "Главное о клане", "Члены клана" },
                new KeyboardButton[] { "История войн", "История рейдов" },
                new KeyboardButton[] { "История розыгрышей", "Активные супер юниты" },
                new KeyboardButton[] { "В главное меню"},
       })
       {
           ResizeKeyboard = true
       };

        ReplyKeyboardMarkup currentClanWarKeyboard =
       new(new[]
       {
                new KeyboardButton[] { "Главное о войне", "Все о войне" },
                new KeyboardButton[] { "В главное меню" },
       })
       {
           ResizeKeyboard = true
       };

        ReplyKeyboardMarkup currentRaidKeyboard =
       new(new[]
       {
                new KeyboardButton[] { "Главное о рейде", "Все о рейде" },
                new KeyboardButton[] { "В главное меню" },
       })
       {
           ResizeKeyboard = true
       };

        ReplyKeyboardMarkup prizeDrawKeboard =
       new(new[]
       {
           new KeyboardButton[] { "Главное о розыгрыше", "Все о розыгрыше" },
           new KeyboardButton[] { "В главное меню" },
       })
       {
           ResizeKeyboard = true
       };

        CustomKeyboards.Add("mainKeyboard", mainKeyboard);
        CustomKeyboards.Add("playerKeyboard", playerKeyboard);
        CustomKeyboards.Add("clanKeyboard", clanKeyboard);
        CustomKeyboards.Add("currentClanWarKeyboard", currentClanWarKeyboard);
        CustomKeyboards.Add("currentRaidKeyboard", currentRaidKeyboard);
        CustomKeyboards.Add("prizeDrawKeboard", prizeDrawKeboard);
    }
}
