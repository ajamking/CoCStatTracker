using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace CoCStatsTrackerBot;

public static class OtherRequestHandler
{

    static OtherRequestHandler()
    {

    }

    public async static Task HandleMessageOtherLvl2(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
                text: "Тут пока пусто, но контент скоро подвезут.");
    }

}
