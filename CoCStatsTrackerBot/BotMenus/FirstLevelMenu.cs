using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot;

public class FirstLevelMenu
{
    public static Dictionary<KeyboardType, ReplyKeyboardMarkup> MainKeyboard { get; } = new Dictionary<KeyboardType, ReplyKeyboardMarkup>();

    static FirstLevelMenu()
    {
        ReplyKeyboardMarkup startMenu =
      new(new[]
      {
           new KeyboardButton[] { "Член клана", "Руководитель", "Прочее" },

      })
      {
          ResizeKeyboard = true
      };

        MainKeyboard.Add(KeyboardType.MainMenu, startMenu);
    }
}
