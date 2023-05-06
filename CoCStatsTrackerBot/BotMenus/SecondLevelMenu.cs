using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot;

public class SecondLevelMenu
{
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> MemberKeyboards { get; } = new Dictionary<KeyboardType, ReplyKeyboardMarkup>();
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> LeaderKeyboards { get; } = new Dictionary<KeyboardType, ReplyKeyboardMarkup>();
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> OtherKyboards { get; } = new Dictionary<KeyboardType, ReplyKeyboardMarkup>();

    static SecondLevelMenu()
    {
        ReplyKeyboardMarkup memberKeyboard = new(new[]
              {
             new KeyboardButton[] { "Игрок", "Клан" },
             new KeyboardButton[] { "Текущая война", "Текущий рейд" },
             new KeyboardButton[] { "Текущий розыгрыш", "Назад" }
       })
        {
            ResizeKeyboard = true
        };

        ReplyKeyboardMarkup leaderKeyboard = new(new[]
         {
             new KeyboardButton[] { "Удалить текущую войну", "Добавить текущую войну" },
             new KeyboardButton[] { "Удалить текущий рейд", "Добавить текущий рейд" },
             new KeyboardButton[] { "Удалить отслеживаемый клан", "Добавить отслеживаемый клан" },
             new KeyboardButton[] { "Удалить текущий розыгрыш", "Добавить розыгрыш" },
             new KeyboardButton[] { "Обновить информацию об игроках", "Назад" }
       })
        {
            ResizeKeyboard = true
        };

        ReplyKeyboardMarkup otherInfoKeyboard = new(new[]
        {
             new KeyboardButton[] { "Советы по использованию бота", "Правила клана" },
             new KeyboardButton[] { "Ссылки на кланы и ТГ", "Полезные ресурсы по CoC" },
             new KeyboardButton[] { "Оставить отзыв", "Назад" }
       })
        {
            ResizeKeyboard = true
        };

        MemberKeyboards.Add(KeyboardType.Member, memberKeyboard);
        LeaderKeyboards.Add(KeyboardType.Leader, leaderKeyboard);
        OtherKyboards.Add(KeyboardType.OtherInfo, otherInfoKeyboard);
    }
}
