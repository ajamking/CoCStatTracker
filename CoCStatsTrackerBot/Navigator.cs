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
    public static Dictionary<long, MenuLevels> CurrentUserMenuLevel = new Dictionary<long, MenuLevels>();

    public static List<BaseMenu> Menues = new Menues().AllMenues;

    public async static Task HandleMessage(ITelegramBotClient botClient, Message message)
    {
        CurrentUserMenuLevel.TryAdd(message.Chat.Id, MenuLevels.Main0);

        if (message.Text == "Назад" && CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
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
                     MenuLevels.CurrentRaidInfo2 or MenuLevels.CurrentPrizedrawInfo2:
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
                case MenuLevels.ClanWarsHistory3 or MenuLevels.ClanRaidsHistory3 or MenuLevels.ClanPrizeDrawHistory3:
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "Воспользуйтесь меню.",
                        replyMarkup: Menues.First(x => x.MenuLevel == MenuLevels.ClanInfo2).Keyboard);

                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.ClanInfo2;

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
                case string msg when MemberRequestHandler.ClanRegex.IsMatch(msg):
                    {
                        if (MemberRequestHandler.LastUserClanTags.ContainsKey(message.Chat.Id))
                        {
                            MemberRequestHandler.LastUserClanTags[message.Chat.Id] = msg;
                        }
                        else
                        {
                            MemberRequestHandler.LastUserClanTags.Add(message.Chat.Id, msg);
                        }

                        await botClient.SendTextMessageAsync(message.Chat.Id, text: "Тег клана задан в корректной форме!");

                        //await MemberRequestHandler.HandleClanInfoLvl2(botClient, message, );

                        return;
                    }
                case string msg when MemberRequestHandler.PlayerRegex.IsMatch(msg):
                    {
                        if (MemberRequestHandler.LastUserPlayerTags.ContainsKey(message.Chat.Id))
                        {
                            MemberRequestHandler.LastUserPlayerTags[message.Chat.Id] = msg;
                        }
                        else
                        {
                            MemberRequestHandler.LastUserPlayerTags.Add(message.Chat.Id, msg);
                        }

                        await botClient.SendTextMessageAsync(message.Chat.Id, text: "Тег игрока задан в корректной форме!");

                        //await MemberRequestHandler.HandlePlayerInfoLvl2(botClient, message,);

                        return;
                    }

                default:
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, text: "Если вы пытались задать тег игрока или клана," +
                          "то не вышло, задайте тег в корректной форме или выберите другуо опцию из меню.");

                        break;
                    }

            }
        }

        foreach (var item in Menues)
        {
            if (item.Header == message.Text)
            {
                switch (item.MenuLevel)
                {
                    case MenuLevels.Main0:
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id,
                               text: "Приветствую! Я уникальный бот вашего клана в Clash of Clans. Выберите интересующий вас вариант из меню",
                               replyMarkup: item.Keyboard);

                            CurrentUserMenuLevel[message.Chat.Id] = item.MenuLevel;

                            return;
                        }
                    default:
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id,
                               text: "Выберите интересующий пункт из меню",
                               replyMarkup: item.Keyboard);

                            CurrentUserMenuLevel[message.Chat.Id] = item.MenuLevel;

                            return;
                        }
                }
            }
        }

        foreach (var item in Menues)
        {
            if (item.KeyWords.Contains(message.Text))
            {
                switch (item.MenuLevel)
                {
                    case MenuLevels.PlayerInfo2:
                        {
                            await MemberRequestHandler.HandlePlayerInfoLvl2(botClient, message, item.KeyWords);

                            if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                            {
                                CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.PlayerInfo2;
                            }

                            return;
                        }
                    case MenuLevels.ClanInfo2:
                        {
                            await MemberRequestHandler.HandleClanInfoLvl2(botClient, message, item.KeyWords);

                            if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                            {
                                CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.ClanInfo2;
                            }

                            return;
                        }
                    case MenuLevels.CurrentWarInfo2:
                        {
                            await MemberRequestHandler.HandleCurrentWarInfoLvl2(botClient, message, item.KeyWords);

                            if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                            {
                                CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.CurrentWarInfo2;
                            }

                            return;
                        }
                    case MenuLevels.CurrentRaidInfo2:
                        {
                            await MemberRequestHandler.HandleCurrentRaidInfoLvl2(botClient, message, item.KeyWords);

                            if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                            {
                                CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.CurrentRaidInfo2;
                            }

                            return;
                        }
                    case MenuLevels.CurrentPrizedrawInfo2:
                        {
                            await MemberRequestHandler.HandleCurrentPrizeDrawInfoLvl2(botClient, message, item.KeyWords);

                            if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                            {
                                CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.CurrentPrizedrawInfo2;
                            }

                            return;
                        }

                    case MenuLevels.PlayerWarStatistics3:
                        {
                            await MemberRequestHandler.HandlePlayerWarStatisticsLvl3(botClient, message, item.KeyWords);

                            if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                            {
                                CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.PlayerWarStatistics3;
                            }

                            return;
                        }
                    case MenuLevels.PlayerRaidStatistics3:
                        {
                            await MemberRequestHandler.HandlePlayerRaidStatisticsLvl3(botClient, message, item.KeyWords);

                            if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                            {
                                CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.PlayerRaidStatistics3;
                            }

                            return;
                        }
                    case MenuLevels.PlayerArmy3:
                        {
                            await MemberRequestHandler.HandlePlayerArmyLvl3(botClient, message, item.KeyWords);

                            if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                            {
                                CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.PlayerArmy3;
                            }

                            return;
                        }
                    case MenuLevels.ClanWarsHistory3:
                        {
                            await MemberRequestHandler.HandleClanWarHistoryLvl3(botClient, message, item.KeyWords);

                            if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                            {
                                CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.ClanWarsHistory3;
                            }

                            return;
                        }
                    case MenuLevels.ClanRaidsHistory3:
                        {
                            await MemberRequestHandler.HandleClanRaidHistoryLvl3(botClient, message, item.KeyWords);

                            if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                            {
                                CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.ClanRaidsHistory3;
                            }

                            return;
                        }
                    case MenuLevels.ClanPrizeDrawHistory3:
                        {
                            await MemberRequestHandler.HandleClanPrizeDrawHistoryLvl3(botClient, message, item.KeyWords);

                            if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                            {
                                CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.ClanPrizeDrawHistory3;
                            }

                            return;
                        }
                    case MenuLevels.CurrentDistrictStatistics3:
                        {
                            await MemberRequestHandler.HandleCurrentDistrictStatistics3(botClient, message, item.KeyWords);

                            if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                            {
                                CurrentUserMenuLevel[message.Chat.Id] = MenuLevels.CurrentDistrictStatistics3;
                            }

                            return;
                        }
                    default:
                        {
                            continue;
                        }
                }
            }

            //else
            //{

            //    break;
            //}
        }

        await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: $"Вы сказали {message.Text}, но я еще не знаю таких сложных вещей. 🥺\n" +
                      $"Выберите что-то из меню или введите корректный тег игрока/клана 😁");

    }
}
