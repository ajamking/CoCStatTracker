using Domain.Entities;
using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Text.RegularExpressions;
using CoCStatsTrackerBot.Menue;

namespace CoCStatsTrackerBot;

public class UserInfo
{
    public long UserId { get; set; }
    public MenuLevels MenuLevel { get; set; }
    public string LastMenuHeader { get; set; }

    public UserInfo(long userId, MenuLevels menuLevel, string lastMenuLevel)
    {
        UserId = userId;
        MenuLevel = menuLevel;
        LastMenuHeader = lastMenuLevel;
    }
}

public static class NavigatorNew
{
    public static List<UserInfo> UserInfos = new List<UserInfo>();

    public async static Task HandleMessage(ITelegramBotClient botClient, Message message)
    {
        if (!UserInfos.Any(x => x.UserId == message.Chat.Id))
        {
            UserInfos.Add(new UserInfo(message.Chat.Id, MenuLevels.Main0, message.Text));
        }

        if (message.Text == "Назад" && UserInfos.Any(x => x.UserId == message.Chat.Id))
        {
            var user = UserInfos.Where(x => x.UserId == message.Chat.Id).First();

            switch (user.MenuLevel)
            {
                case MenuLevels.Main0:
                    {

                        return;
                    }
                default:
            }
        }
    }

}
