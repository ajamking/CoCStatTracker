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
using System.Security.Cryptography.X509Certificates;

namespace CoCStatsTrackerBot;

public static class Navigator
{


    public static List<BaseMenu> Menues = new Menues().AllMenues;

    public static Dictionary<long, MenuLevels> CurrentUserMenuLevel = new Dictionary<long, MenuLevels>();

    public async static Task HandleMessage(ITelegramBotClient botClient, Message message)
    {
        CurrentUserMenuLevel.TryAdd(message.Chat.Id, MenuLevels.Main0);

        if (message.Text.ToLower() == "назад")
        {
            switch (CurrentUserMenuLevel[message.Chat.Id])
            {
                case MenuLevels.Member1:
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "Воспользуйтесь меню.",
                        replyMarkup: Menues.First(x => x.MenuLevel == MenuLevels.Main0).Keyboard);

                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.Main0;

                        return;
                    }
                case MenuLevels.PlayerInfo2 or MenuLevels.ClanInfo2 or MenuLevels.CurrentWarInfo2 or
                     MenuLevels.CurrentRaidInfo2:
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "Воспользуйтесь меню.",
                        replyMarkup: Menues.First(x => x.MenuLevel == MenuLevels.Member1).Keyboard);

                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.Member1;

                        return;
                    }
                case MenuLevels.PlayerWarStatistics3 or MenuLevels.PlayerRaidStatistics3 or MenuLevels.PlayerArmy3:
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "Воспользуйтесь меню.",
                        replyMarkup: Menues.First(x => x.MenuLevel == MenuLevels.PlayerInfo2).Keyboard);

                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.PlayerInfo2;

                        return;
                    }
                case MenuLevels.ClanWarsHistory3 or MenuLevels.ClanRaidsHistory3:
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "Воспользуйтесь меню.",
                        replyMarkup: Menues.First(x => x.MenuLevel == MenuLevels.ClanInfo2).Keyboard);

                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.ClanInfo2;

                        return;
                    }
                case MenuLevels.CurrentDistrictStatistics3:
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "Воспользуйтесь меню.",
                        replyMarkup: Menues.First(x => x.MenuLevel == MenuLevels.CurrentRaidInfo2).Keyboard);

                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.CurrentRaidInfo2;

                        return;
                    }
                default:
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "Вы в начальной директории, попробуйте выбрать пункт из меню.",
                        replyMarkup: Menues.First(x => x.MenuLevel == MenuLevels.Main0).Keyboard);

                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.Main0;

                        return;
                    }
            }
        }

        if (message.Text.Contains('#'))
        {
            switch (message.Text)
            {
                case string msg when MemberRequestHandler.TagRegex.IsMatch(msg):
                    {
                        if (MemberRequestHandler.UsersLastTags.ContainsKey(message.Chat.Id))
                        {
                            MemberRequestHandler.UsersLastTags[message.Chat.Id] = msg;
                        }
                        else
                        {
                            MemberRequestHandler.UsersLastTags.Add(message.Chat.Id, msg);
                        }

                        await botClient.SendTextMessageAsync(message.Chat.Id, text: "Тег задан в корректной форме!");

                        return;
                    }
                default:
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, text: "Если вы пытались задать тег игрока или клана, " +
                          "то не вышло, задайте тег в корректной форме или выберите другуо опцию из меню.");

                        return;
                    }
            }
        }

        foreach (var menu in Menues)
        {
            if (menu.Header == message.Text)
            {
                switch (menu.MenuLevel)
                {
                    case MenuLevels.Main0:
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id,
                               text: "Приветствую! Я уникальный бот вашего клана в Clash of Clans. Выберите интересующий вас вариант из меню",
                               replyMarkup: menu.Keyboard);

                            CurrentUserMenuLevel[message.Chat.Id] = menu.MenuLevel;

                            return;
                        }
                    default:
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id,
                               text: "Выберите интересующий пункт из меню",
                               replyMarkup: menu.Keyboard);

                            CurrentUserMenuLevel[message.Chat.Id] = menu.MenuLevel;

                            return;
                        }
                }
            }


        }

        foreach (var menu in Menues)
        {
            if (menu.KeyWords.Contains(message.Text) && menu.MenuLevel == CurrentUserMenuLevel[message.Chat.Id])
            {
                switch (CurrentUserMenuLevel[message.Chat.Id])
                {
                    case MenuLevels.PlayerInfo2:
                        {
                            await MemberRequestHandler.HandlePlayerInfoLvl2(botClient, message, menu.KeyWords);

                            return;
                        }
                    case MenuLevels.ClanInfo2:
                        {
                            await MemberRequestHandler.HandleClanInfoLvl2(botClient, message, menu.KeyWords);

                            return;
                        }
                    case MenuLevels.CurrentWarInfo2:
                        {
                            await MemberRequestHandler.HandleCurrentWarInfoLvl2(botClient, message, menu.KeyWords);

                            return;
                        }
                    case MenuLevels.CurrentRaidInfo2:
                        {
                            await MemberRequestHandler.HandleCurrentRaidInfoLvl2(botClient, message, menu.KeyWords);

                            return;
                        }



                    case MenuLevels.PlayerWarStatistics3:
                        {
                            await MemberRequestHandler.HandlePlayerWarStatisticsLvl3(botClient, message, menu.KeyWords);

                            return;
                        }
                    case MenuLevels.PlayerRaidStatistics3:
                        {
                            await MemberRequestHandler.HandlePlayerRaidStatisticsLvl3(botClient, message, menu.KeyWords);

                            return;
                        }
                    case MenuLevels.PlayerArmy3:
                        {
                            await MemberRequestHandler.HandlePlayerArmyLvl3(botClient, message, menu.KeyWords);

                            return;
                        }
                    case MenuLevels.ClanWarsHistory3:
                        {
                            await MemberRequestHandler.HandleClanWarHistoryLvl3(botClient, message, menu.KeyWords);

                            return;
                        }
                    case MenuLevels.ClanRaidsHistory3:
                        {
                            await MemberRequestHandler.HandleClanRaidHistoryLvl3(botClient, message, menu.KeyWords);

                            return;
                        }
                    case MenuLevels.CurrentDistrictStatistics3:
                        {
                            await MemberRequestHandler.HandleCurrentDistrictStatistics3(botClient, message, menu.KeyWords);

                            return;
                        }
                    default:
                        {
                            Console.WriteLine("Ошбика при орпеделении уровня меню сообщения");
                            continue;
                        }
                }
            }
        }

        await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: $"Вы сказали {message.Text}, но я еще не знаю таких сложных вещей. 🥺\n" +
                      $"Выберите что-то из меню или введите корректный тег игрока/клана 😁");

    }
}
