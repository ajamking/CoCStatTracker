using CoCStatsTrackerBot.Menu;
using CoCStatsTrackerBot.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using CoCStatsTrackerBot.Requests.RequestHandlers;
using System.Text.RegularExpressions;
using CoCStatsTrackerBot.Helpers;
using CoCStatsTracker;

namespace CoCStatsTrackerBot;

public static class Navigation
{
    private static Regex _tagRegex { get; set; } = new Regex(@"^#(\w{6,9})$");

    private static List<BaseRequestHandler> _allRequestHandlers = AllRequestHandlersConstructor.AllRequestHandlers;

    private static List<BotUser> _botUsers = new List<BotUser>();

    private static readonly string _botHolderToken = System.IO.File.ReadAllText(@"./../../../../CustomSolutionElements/MyPersonalKey.txt");

    private static Func<BotUser, bool>[] _handlers = new[]
    {
        BotHolderTokenAction,
        BotHolderEnterKeyToMergeAction,
        BotHolderEnterNewClanTagToAddAction,
        AnyBackInMenuAction,
        AdminsKeyAction,
        AdminEnterClanTagAction,
        CommonUserEnterTagAction,
        AnyRequestHeaderMatchAction,
        DefaultAction
    };

    public static void Execute(ITelegramBotClient botClient, Message message)
    {
        TryAddBotUser(botClient, message);

        var activeBotUser = _botUsers.First(x => x.ChatId == message.Chat.Id);

        UpdateUsersRHParametersMesssage(activeBotUser, message);

        var isProcessed = false;

        foreach (var handler in _handlers)
        {
            isProcessed = handler.Invoke(activeBotUser);

            if (isProcessed)
            {
                break;
            }
        }
    }

    #region Actions
    private static bool BotHolderTokenAction(BotUser activeBotUser)
    {
        if (activeBotUser.RequestHadnlerParameters.Message.Text == _botHolderToken)
        {
            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                                StylingHelper.MakeItStyled($"Вы определены как владелец бота.", UiTextStyle.Default));

            UpsertBotHolderProperty(activeBotUser.RequestHadnlerParameters.Message);

            return true;
        }

        return false;
    }

    private static bool BotHolderEnterKeyToMergeAction(BotUser activeBotUser)
    {
        if (activeBotUser.RequestHadnlerParameters.Message.Text.Contains('@') && (activeBotUser.CurrentMenuLevel is MenuLevel.DeveloperMenu2))
        {
            UpsertBotHolderNewAdninKeToMerge(activeBotUser.RequestHadnlerParameters.Message);

            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                            StylingHelper.MakeItStyled($"Новый токен для клана принят, можно устанавливать.", UiTextStyle.Default));

            return true;
        }

        return false;
    }

    private static bool BotHolderEnterNewClanTagToAddAction(BotUser activeBotUser)
    {
        var messageText = activeBotUser.RequestHadnlerParameters.Message.Text;

        if (messageText.Contains('#') && activeBotUser.CurrentMenuLevel is MenuLevel.DeveloperMenu2 &&
            !GetFromDbQueryHandler.GetAllTrackedClans().Where(x => x.Tag == messageText).Any())
        {
            if (_tagRegex.IsMatch(messageText))
            {
                UpsertBotHolderNewTrackedClanTag(activeBotUser.RequestHadnlerParameters.Message);

                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                                StylingHelper.MakeItStyled($"Новый тег клана прошел проверку, можно добавлять клан.", UiTextStyle.Default));
            }
            else
            {
                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                              StylingHelper.MakeItStyled($"Новый тег клана не прошел проверку, некорректный тег.", UiTextStyle.Default));
            }

            return true;
        }

        return false;
    }

    private static bool AnyBackInMenuAction(BotUser activeBotUser)
    {
        if (activeBotUser.RequestHadnlerParameters.Message.Text.ToLower() != "назад")
        {
            return false;
        }

        switch (activeBotUser.CurrentMenuLevel)
        {
            case MenuLevel.PlayerInfo2 or MenuLevel.ClanInfo2
                 or MenuLevel.CurrentWarInfo2 or MenuLevel.CurrentRaidInfo2:
                {
                    _allRequestHandlers
                        .First(x => x.HandlerMenuLevel == MenuLevel.Member1)
                        .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                    activeBotUser.CurrentMenuLevel = MenuLevel.Member1;

                    return true;
                }
            case MenuLevel.PlayerWarStatistics3 or MenuLevel.PlayerRaidStatistics3
                 or MenuLevel.PlayerArmy3:
                {
                    _allRequestHandlers
                       .First(x => x.HandlerMenuLevel == MenuLevel.PlayerInfo2)
                       .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                    activeBotUser.CurrentMenuLevel = MenuLevel.PlayerInfo2;

                    return true;
                }
            case MenuLevel.ClanWarsHistory3 or MenuLevel.ClanRaidsHistory3:
                {
                    _allRequestHandlers
                       .First(x => x.HandlerMenuLevel == MenuLevel.ClanInfo2)
                       .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                    activeBotUser.CurrentMenuLevel = MenuLevel.ClanInfo2;

                    return true;
                }
            case MenuLevel.CurrentDistrictStatistics3:
                {

                    _allRequestHandlers
                     .First(x => x.HandlerMenuLevel == MenuLevel.CurrentRaidInfo2)
                     .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                    activeBotUser.CurrentMenuLevel = MenuLevel.CurrentRaidInfo2;

                    return true;
                }
            case MenuLevel.DeveloperMenu2 or MenuLevel.LeaderAddMenu2
                 or MenuLevel.LeaderUpdateMenu2 or MenuLevel.LeaderDeleteMenu2:
                {
                    _allRequestHandlers
                    .First(x => x.HandlerMenuLevel == MenuLevel.Leader1)
                    .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                    activeBotUser.CurrentMenuLevel = MenuLevel.Leader1;

                    return true;
                }
            case MenuLevel.LeaderDeleteRaidsMenu3 or MenuLevel.LeaderDeleteClanWarsMenu3:
                {
                    _allRequestHandlers
                    .First(x => x.HandlerMenuLevel == MenuLevel.LeaderDeleteMenu2)
                    .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                    activeBotUser.CurrentMenuLevel = MenuLevel.LeaderDeleteMenu2;

                    return true;
                }
            default:
                {
                    _allRequestHandlers
                     .First(x => x.HandlerMenuLevel == MenuLevel.Main0)
                     .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                    activeBotUser.CurrentMenuLevel = MenuLevel.Main0;

                    return true;
                }

        }
    }

    private static bool AdminsKeyAction(BotUser activeBotUser)
    {
        if (activeBotUser.RequestHadnlerParameters.Message.Text.CheckClanWithAdminsKeyExistsInDb())
        {
            UpsertLastAdminsKey(activeBotUser.RequestHadnlerParameters.Message);

            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                            StylingHelper.MakeItStyled($"Токен авторизации задан корректно, выберите пункт из меню.", UiTextStyle.Default));

            return true;
        }

        return false;
    }

    private static bool AdminEnterClanTagAction(BotUser activeBotUser)
    {
        if (activeBotUser.RequestHadnlerParameters.Message.Text.Contains('#') && (activeBotUser.CurrentMenuLevel is MenuLevel.Leader1
          or MenuLevel.DeveloperMenu2 or MenuLevel.LeaderAddMenu2 or MenuLevel.LeaderUpdateMenu2 or MenuLevel.LeaderDeleteMenu2
          or MenuLevel.LeaderDeleteRaidsMenu3 or MenuLevel.LeaderDeleteClanWarsMenu3))
        {
            if (TagsConditionChecker.CheckClanIsAllowedToMerge(activeBotUser.RequestHadnlerParameters) || activeBotUser.IsBotHolder)
            {
                UpsertLastClanTagToMergeMessage(activeBotUser.RequestHadnlerParameters.Message);

                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                                StylingHelper.MakeItStyled($"Тег задан корректно! Редактируемый клан определен.", UiTextStyle.Default));
            }
            else
            {
                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                               StylingHelper.MakeItStyled($"Клан с таким тегом не отслеживается или у вас нет прав для его редактиврования.", UiTextStyle.Default));
            }

            return true;
        }

        return false;
    }

    private static bool CommonUserEnterTagAction(BotUser activeBotUser)
    {
        if (!activeBotUser.RequestHadnlerParameters.Message.Text.Contains('#'))
        {
            return false;
        }

        var message = activeBotUser.RequestHadnlerParameters.Message;

        switch (message.Text)
        {
            case string msg when _tagRegex.IsMatch(msg):
                {
                    if (TagsConditionChecker.CheckClanExistInDb(activeBotUser.RequestHadnlerParameters))
                    {
                        UpsertLastClanTagMessage(message);

                        ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                            StylingHelper.MakeItStyled($"Тег задан корректно! У меня есть информация об этом клане.", UiTextStyle.Default));

                        return true;
                    }
                    else if (TagsConditionChecker.CheckMemberExistInDb(activeBotUser.RequestHadnlerParameters))
                    {
                        UpsertLastMemberTagMessage(message);

                        ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                            StylingHelper.MakeItStyled($"Тег задан корректно! У меня есть информация об этом игроке.", UiTextStyle.Default));

                        return true;
                    }
                    else
                    {
                        ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                            StylingHelper.MakeItStyled($"Тег был задан в корректной форме, но ни игрок ни клан с таким тегом" +
                            $" не отслеживаются. Попробуйте ввести другой.", UiTextStyle.Default));

                        return true;
                    }
                }
            default:
                {
                    ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                      StylingHelper.MakeItStyled($"Тег не прошел проверку.\r\nЗадайте тег в корректной форме или выберите другуо опцию из меню.", UiTextStyle.Default));

                    return true;
                }
        }
    }

    private static bool AnyRequestHeaderMatchAction(BotUser activeBotUser)
    {
        if (_allRequestHandlers.Any(x => x.Header == activeBotUser.RequestHadnlerParameters.Message.Text))
        {
            var requestHandler = _allRequestHandlers.First(x => x.Header == activeBotUser.RequestHadnlerParameters.Message.Text);

            //Проверка ТЕГ игрока
            if (string.IsNullOrEmpty(activeBotUser.RequestHadnlerParameters.LastMemberTagMessage) && (requestHandler.HandlerMenuLevel
                is MenuLevel.PlayerInfo2
                or MenuLevel.PlayerWarStatistics3
                or MenuLevel.PlayerRaidStatistics3
                or MenuLevel.PlayerArmy3))
            {
                TagsConditionChecker.SendMemberTagMessageIsEmpty(activeBotUser.RequestHadnlerParameters);
            }
            //Проверка ТЕГ клана
            else if (string.IsNullOrEmpty(activeBotUser.RequestHadnlerParameters.LastClanTagMessage) && (requestHandler.HandlerMenuLevel
                is MenuLevel.ClanInfo2
                or MenuLevel.ClanWarsHistory3
                or MenuLevel.ClanRaidsHistory3
                or MenuLevel.CurrentDistrictStatistics3
                or MenuLevel.CurrentWarInfo2
                or MenuLevel.CurrentRaidInfo2))
            {
                TagsConditionChecker.SendClanTagMessageIsEmpty(activeBotUser.RequestHadnlerParameters);
            }
            //Проверка токена авторизации для главы клана
            else if (string.IsNullOrEmpty(activeBotUser.RequestHadnlerParameters.AdminsKey) && (
                    requestHandler.HandlerMenuLevel
                     is MenuLevel.Leader1
                     or MenuLevel.DeveloperMenu2
                     or MenuLevel.LeaderAddMenu2
                     or MenuLevel.LeaderUpdateMenu2
                     or MenuLevel.LeaderDeleteMenu2
                     or MenuLevel.LeaderDeleteRaidsMenu3
                     or MenuLevel.LeaderDeleteClanWarsMenu3))
            {
                TagsConditionChecker.SendAdminsKeyIsEmpty(activeBotUser.RequestHadnlerParameters);
            }
            //Проверка на токен авторизации создателя бота
            else if (requestHandler.HandlerMenuLevel is MenuLevel.DeveloperMenu2 && activeBotUser.IsBotHolder is false)
            {
                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
              StylingHelper.MakeItStyled($"У вас недостаточно прав для вызова этого меню.", UiTextStyle.Default));
            }
            //Проверка установленного для изменения клана
            else if (string.IsNullOrEmpty(activeBotUser.RequestHadnlerParameters.LastClanTagToMerge) &&
                requestHandler.HandlerMenuLevel is MenuLevel.LeaderAddMenu2 or MenuLevel.LeaderUpdateMenu2 or MenuLevel.LeaderDeleteMenu2)
            {
                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
            StylingHelper.MakeItStyled($"Для использования этих функций необходимо сначала выбрать редактируемый клан.", UiTextStyle.Default));
            }
            else
            {
                requestHandler.Execute(activeBotUser.RequestHadnlerParameters);

                UpdateUserMenuLevels(activeBotUser, requestHandler);
            }

            return true;
        }

        return false;
    }

    private static bool DefaultAction(BotUser activeBotUser)
    {
        ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
            StylingHelper.MakeItStyled($"Вы сказали {activeBotUser.RequestHadnlerParameters.Message.Text}, но я еще не знаю таких сложных вещей.🥺 \r\n" +
            $"Выберите что-то из меню или введите корректный тег игрока или кална.😁", UiTextStyle.Default));

        return true;
    }
    #endregion


    #region EditBotUserProperties
    private static void TryAddBotUser(ITelegramBotClient telegramBotClient, Message message)
    {
        if (!_botUsers.Any(x => x.ChatId == message.Chat.Id))
        {
            _botUsers.Add(new BotUser(telegramBotClient, message));
        }
    }

    private static void UpdateUserMenuLevels(BotUser activeBotUser, BaseRequestHandler requestHandler)
    {
        activeBotUser.PreviousMenuLevel = activeBotUser.CurrentMenuLevel;

        activeBotUser.CurrentMenuLevel = requestHandler.HandlerMenuLevel;

        if (requestHandler.HandlerMenuLevel is MenuLevel.Main0)
        {
            requestHandler.ShowKeyboard(activeBotUser.RequestHadnlerParameters);
        }
        if (activeBotUser.CurrentMenuLevel != activeBotUser.PreviousMenuLevel)
        {
            requestHandler.ShowKeyboard(activeBotUser.RequestHadnlerParameters);
        }
    }

    private static void UpdateUsersRHParametersMesssage(BotUser activeBotUser, Message message)
    {
        activeBotUser.RequestHadnlerParameters.Message = message;
    }


    private static void UpsertLastClanTagMessage(Message message)
    {
        _botUsers.First(x => x.ChatId == message.Chat.Id).RequestHadnlerParameters.LastClanTagMessage = message.Text;
    }

    private static void UpsertLastMemberTagMessage(Message message)
    {
        _botUsers.First(x => x.ChatId == message.Chat.Id).RequestHadnlerParameters.LastMemberTagMessage = message.Text;
    }

    private static void UpsertLastClanTagToMergeMessage(Message message)
    {
        _botUsers.First(x => x.ChatId == message.Chat.Id).RequestHadnlerParameters.LastClanTagToMerge = message.Text;
    }

    private static void UpsertLastAdminsKey(Message message)
    {
        _botUsers.First(x => x.ChatId == message.Chat.Id).RequestHadnlerParameters.AdminsKey = message.Text;
    }

    private static void UpsertBotHolderProperty(Message message)
    {
        _botUsers.First(x => x.ChatId == message.Chat.Id).IsBotHolder = true;
    }

    private static void UpsertBotHolderNewAdninKeToMerge(Message message)
    {
        _botUsers.First(x => x.ChatId == message.Chat.Id).RequestHadnlerParameters.AdminKeyToMerge = message.Text;
    }

    private static void UpsertBotHolderNewTrackedClanTag(Message message)
    {
        _botUsers.First(x => x.ChatId == message.Chat.Id).RequestHadnlerParameters.TagToAddClan = message.Text;
    }
    #endregion
}