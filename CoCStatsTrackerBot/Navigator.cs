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

namespace CoCStatsTrackerBot;

public static class Navigator
{
    public static Dictionary<long, MenuLevel> CurrentUserMenuLevel = new Dictionary<long, MenuLevel>();

    public async static Task HandleMessage(ITelegramBotClient botClient, Message message)
    {
        CurrentUserMenuLevel.TryAdd(message.Chat.Id, MenuLevel.Main);

        if (message.Text == "Назад" && CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
        {
            switch (CurrentUserMenuLevel[message.Chat.Id])
            {
                case MenuLevel.Member2:
                    {
                        await HandleMessageLvl1(botClient, message, true);

                        if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                        {
                            CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Main;
                        }

                        return;

                    }
                case MenuLevel.Member3:
                    {
                        await MemberRequestHandler.HandleMessageMemberLvl2(botClient, message, true);

                        if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                        {
                            CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member2;
                        }

                        return;
                    }
                case MenuLevel.Member4:
                    {
                        await MemberRequestHandler.HandlePlayerInfoLvl3(botClient, message, true);

                        if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                        {
                            CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member3;
                        }

                        return;
                    }

                case MenuLevel.Leader2:
                    {
                        await HandleMessageLvl1(botClient, message, true);

                        if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                        {
                            CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Main;
                        }

                        return;

                    }
                case MenuLevel.Leader3:
                    {
                        await LeaderRequestHandler.HandleMessageLeaderLvl2(botClient, message, true);

                        if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                        {
                            CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Leader2;
                        }

                        return;
                    }

                default:
                    {
                        await HandleMessageLvl1(botClient, message, true);

                        if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                        {
                            CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Main;
                        }

                        return;
                    }
            }
        }
        else if (message.Text == "Назад")
        {
            await HandleMessageLvl1(botClient, message, true);

            if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
            {
                CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Main;
            }

            return;
        }

        switch (message.Text)
        {
            case "/start":
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "Приветствую! Я многофункциональный бот. Выберите интересующий вас вариант из меню",
                        replyMarkup: Menu.MainKeyboards[0]);

                    return;
                }
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


                    await MemberRequestHandler.HandleClanInfoLvl3(botClient, message, false);

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


                    await MemberRequestHandler.HandlePlayerInfoLvl3(botClient, message, false);

                    return;
                }
           


            case "Член клана":
                {
                    await MemberRequestHandler.HandleMessageMemberLvl2(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member2;
                    }

                    return;
                }
            case "Руководитель":
                {
                    await LeaderRequestHandler.HandleMessageLeaderLvl2(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Leader2;
                    }

                    return;
                }
            case "Прочее":
                {
                    await OtherRequestHandler.HandleMessageOtherLvl2(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Other2;
                    }

                    return;
                }

            case string msg when Menu.Lvl2MemberWords.Contains(msg):
                {
                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member3;
                    }

                    switch (msg)
                    {
                        case "Игрок":
                            await MemberRequestHandler.HandlePlayerInfoLvl3(botClient, message, true);
                            return;
                        case "Клан":
                            await MemberRequestHandler.HandleClanInfoLvl3(botClient, message, true);
                            return;
                        case "Текущая война":
                            await MemberRequestHandler.HandleCurrentWarInfoLvl3(botClient, message, true);
                            return;
                        case "Текущий рейд":
                            await MemberRequestHandler.HandleCurrentRaidInfoLvl3(botClient, message, true);
                            return;
                        case "Текущий розыгрыш":
                            await MemberRequestHandler.HandleCurrentPrizeDrawInfoLvl3(botClient, message, true);
                            return;

                        default:
                            await botClient.SendTextMessageAsync(message.Chat.Id, text: "Сломался в case Lvl2MemberWords");
                            return;
                    }
                }

            case string msg when Menu.Lvl3PlayerInfoWords.Contains(msg):
                {
                    await MemberRequestHandler.HandlePlayerInfoLvl3(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member3;
                    }

                    return;
                }
            case string msg when Menu.Lvl3ClanInfoWords.Contains(msg):
                {
                    await MemberRequestHandler.HandleClanInfoLvl3(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member3;
                    }

                    return;
                }
            case string msg when Menu.Lvl3CurrentWarInfoWords.Contains(msg):
                {
                    await MemberRequestHandler.HandleCurrentWarInfoLvl3(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member3;
                    }

                    return;
                }
            case string msg when Menu.Lvl3CurrentRaidInfoWords.Contains(msg):
                {
                    await MemberRequestHandler.HandleCurrentRaidInfoLvl3(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member3;
                    }

                    return;
                }
            case string msg when Menu.Lvl3CurrentPrizeDrawInfoWords.Contains(msg):
                {
                    await MemberRequestHandler.HandleCurrentPrizeDrawInfoLvl3(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member3;
                    }

                    return;
                }

            case string msg when Menu.Lvl4WarStatisticsWords.Contains(msg):
                {
                    await MemberRequestHandler.HandlePlayerWarStatisticsLvl4(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member4;
                    }

                    return;
                }
            case string msg when Menu.Lvl4WarHistoryWords.Contains(msg):
                {
                    await MemberRequestHandler.HandleClanWarHistoryLvl4(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member4;
                    }

                    return;
                }
            case string msg when Menu.Lvl4RaidStatisticsWords.Contains(msg):
                {
                    await MemberRequestHandler.HandleRaidStatisticsLvl4(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member4;
                    }

                    return;
                }
            case string msg when Menu.Lvl4RaidHistoryWords.Contains(msg):
                {
                    await MemberRequestHandler.HandleClanRaidHistoryLvl4(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member4;
                    }

                    return;
                }
            case string msg when Menu.Lvl4ArmyWords.Contains(msg):
                {
                    await MemberRequestHandler.HandleArmyLvl4(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member4;
                    }

                    return;
                }
            case string msg when Menu.Lvl4PrizeDrawHistoryWords.Contains(msg):
                {
                    await MemberRequestHandler.HandleClanPrizeDrawHistoryLvl4(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member4;
                    }

                    return;
                }
            case string msg when Menu.Lvl4DistrictsStatisticsWords.Contains(msg):
                {
                    await MemberRequestHandler.HandleClanDistrictStatisticsLvl4(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Member4;
                    }

                    return;
                }


            case string msg when Menu.Lvl2LeaderWords.Contains(msg):
                {
                    await LeaderRequestHandler.HandleMessageLeaderLvl3(botClient, message); //Под вопросом какая функция

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Leader2;
                    }

                    return;
                }

            case string msg when Menu.Lvl3LeaderChangeConfirmationWords.Contains(msg):
                {
                    await LeaderRequestHandler.HandleMessageLeaderLvl3(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Leader3;
                    }

                    return;
                }


            case string msg when Menu.Lvl2OtherWords.Contains(msg):
                {
                    await OtherRequestHandler.HandleMessageOtherLvl2(botClient, message);

                    if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
                    {
                        CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Other2;
                    }

                    return;
                }

            default:
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Вы сказали \"{message.Text}\", " +
                    $"но я еще не знаю таких сложных вещей. 🥺 Выберите что-то из меню или введите корректный тег игрока/клана 😁");
                    return;
                }
        }
    }

    async static Task HandleMessageLvl1(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        if (CurrentUserMenuLevel.ContainsKey(message.Chat.Id))
        {
            CurrentUserMenuLevel[message.Chat.Id] = MenuLevel.Main;
        }

        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: "Меню",
                        replyMarkup: Menu.MainKeyboards[0]);
    }
}
