using CoCStatsTracker;
using CoCStatsTrackerBot.Helpers;
using CoCStatsTrackerBot.BotMenues;
using CoCStatsTrackerBot.Requests;
using CoCStatsTrackerBot.Requests.RequestHandlers;
using CoCStatsTrackerBot.Requests.RequestHandlers.SlashFunctionHandlers;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot;

public static class Navigation
{
    private static readonly Dictionary<string, BotSlashFunction> _botSlashFunctions = new() {
       { "/group_get_chat_id", BotSlashFunction.GroupGetChatId },
       { "/group_get_raid_short_info", BotSlashFunction.GroupGetRaidShortInfo },
       { "/group_war_short_info", BotSlashFunction.GroupGetWarShortInfo },
       { "/group_war_map", BotSlashFunction.GroupGetWarMap }
    };

    private static Regex TagRegex { get; set; } = new Regex(@"^#(\w{6,9})$");
    private static Regex AdminUsersNameRegex { get; set; } = new Regex(@"^#(\w{6,9})-@[\w_]");

    private static readonly List<BotUser> _botUsers = new();

    private static readonly List<BaseRequestHandler> _allRequestHandlers = AllRequestHandlersConstructor.AllRequestHandlers;

    private static readonly string _botHolderToken = "gamabunta0707";

    private static readonly Func<BotUser, bool>[] _handlers = new[]
    {
        AnyRequestHeaderMatchAction,
        AnyBackInMenuAction,
        BotHolderEnterNewClanTagToAddAction,
        AdminEnterMemberUserNameAction,
        AdminEnterClanTagAction,
        UserEnterTagAction,
        BotHolderTokenAction,
        BotHolderEnterKeyToMergeAction,
        BotHolderOrAdminEnterClanChatIdAction,
        AdminsKeyAction,
        HoldSlashFuncAction,
        FeedBackAction,
        AdminBanUserAction,
        IsAliveAction,
        TrashMessageAction
    };

    public static void Execute(ITelegramBotClient botClient, Message message)
    {
        TryAddBotUser(botClient, message);

        var activeBotUser = _botUsers.First(x => x.ChatId == message.Chat.Id);

        UpdateUsersRHParametersMesssage(activeBotUser, message);

        foreach (var handler in _handlers)
        {
            if (handler.Invoke(activeBotUser))
            {
                break;
            }
        }
    }

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
        if (activeBotUser.RequestHadnlerParameters.Message.Text.Contains('$') && (activeBotUser.CurrentMenuLevel is MenuLevel.DeveloperMenu2))
        {
            UpsertBotHolderNewAdninKeyToMerge(activeBotUser.RequestHadnlerParameters.Message);

            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                            StylingHelper.MakeItStyled($"Новый токен для клана принят, можно устанавливать.", UiTextStyle.Default));

            return true;
        }

        return false;
    }

    private static bool BotHolderOrAdminEnterClanChatIdAction(BotUser activeBotUser)
    {
        if (activeBotUser.RequestHadnlerParameters.Message.Text.Contains('*') && (activeBotUser.CurrentMenuLevel is MenuLevel.DeveloperMenu2 or MenuLevel.LeaderTgGroupCustomize2))
        {
            UpsertBotHolderNewTrackedClanChatId(activeBotUser.RequestHadnlerParameters.Message);

            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                            StylingHelper.MakeItStyled($"Новый ChatId для клана принят, можно устанавливать.", UiTextStyle.Default));

            return true;
        }

        return false;
    }

    private static bool BotHolderEnterNewClanTagToAddAction(BotUser activeBotUser)
    {
        var messageText = activeBotUser.RequestHadnlerParameters.Message.Text;

        if (messageText.Contains('#') && activeBotUser.CurrentMenuLevel is MenuLevel.DeveloperMenu2 &&
            !GetFromDbQueryHandler.GetAllTrackedClansUi().Where(x => x.Tag == messageText).Any())
        {
            if (TagRegex.IsMatch(messageText))
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
                 or MenuLevel.LeaderUpdateMenu2 or MenuLevel.LeaderDeleteMenu2 or MenuLevel.LeaderTgGroupCustomize2:
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
            case MenuLevel.Layouts2:
                {
                    _allRequestHandlers
                    .First(x => x.HandlerMenuLevel == MenuLevel.Other1)
                    .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                    activeBotUser.CurrentMenuLevel = MenuLevel.Other1;

                    return true;
                }
            case MenuLevel.BbLayouts3 or MenuLevel.ThLayouts3:
                {
                    _allRequestHandlers
                    .First(x => x.HandlerMenuLevel == MenuLevel.Layouts2)
                    .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                    activeBotUser.CurrentMenuLevel = MenuLevel.Layouts2;

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
                            StylingHelper.MakeItStyled($"Токен авторизации задан корректно, теперь вам доступно меню главы клана.", UiTextStyle.Default));

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
            if (TagsConditionChecker.CheckClanIsAllowedToMerge(activeBotUser.RequestHadnlerParameters) || activeBotUser.RequestHadnlerParameters.IsBotHolder)
            {
                UpsertLastClanTagToMergeMessage(activeBotUser.RequestHadnlerParameters.Message);

                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                                StylingHelper.MakeItStyled($"Тег задан корректно! Редактируемый клан определен.", UiTextStyle.Default));
            }
            else
            {
                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                               StylingHelper.MakeItStyled($"Запрос отклонен. Возможные Причины:\n\n" +
                               $"1. Клан с таким тегом не отслеживается;\n" +
                               $"2. Подписка клана была приостановлена;\n" +
                               $"3. У вас нет прав для редактиврования выбранного клана.", UiTextStyle.Default));
            }

            return true;
        }

        return false;
    }

    private static bool AdminEnterMemberUserNameAction(BotUser activeBotUser)
    {
        var msgText = activeBotUser.RequestHadnlerParameters.Message.Text;

        if (AdminUsersNameRegex.IsMatch(msgText) && activeBotUser.CurrentMenuLevel is MenuLevel.LeaderTgGroupCustomize2)
        {
            try
            {
                var answer = UserNameAdder.TryAddUserNames(activeBotUser.RequestHadnlerParameters.LastClanTagToMerge, msgText);

                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true, answer);

                return true;
            }
            catch (Exception ex)
            {
                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, false, ex.Message);

                return true;
            }
        }

        return false;
    }

    private static bool UserEnterTagAction(BotUser activeBotUser)
    {
        if (!activeBotUser.RequestHadnlerParameters.Message.Text.Contains('#'))
        {
            return false;
        }

        var message = activeBotUser.RequestHadnlerParameters.Message;

        if (TagRegex.IsMatch(message.Text))
        {
            if (GetFromDbQueryHandler.CheckClanExists(activeBotUser.RequestHadnlerParameters.Message.Text))
            {
                UpsertLastClanTagMessage(message);

                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                   StylingHelper.MakeItStyled($"Тег задан корректно! Теперь вы можете получить информацию об этом клане. Выберите интересующий пункт из меню.", UiTextStyle.Default));

                return true;
            }
            else if (GetFromDbQueryHandler.CheckMemberExists(activeBotUser.RequestHadnlerParameters.Message.Text))
            {
                UpsertLastMemberTagMessage(message);

                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                    StylingHelper.MakeItStyled($"Тег задан корректно! Теперь вы можете получить информацию об этом игроке. Выберите интересующий пункт из меню.", UiTextStyle.Default));

                return true;
            }
            else
            {
                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                    StylingHelper.MakeItStyled($"Тег был задан в корректной форме, но ни игрок ни клан с таким тегом не отслеживаются или подписка клана была приостановлена." +
                    $"\n\nЕсли вы хотите воспользоваться услугами бота - обратитесь к администратору (ссылка в шапке профиля бота)." +
                    $"\n\nЕсли вы уверены, что бот отслеживает ваш клан - попробуйте ввести другой тег.", UiTextStyle.Default));


                return true;
            }
        }
        else
        {

            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
              StylingHelper.MakeItStyled($"Тег не прошел проверку.\r\nЗадайте тег в корректной форме или выберите другуо опцию из меню.", UiTextStyle.Default));

            return true;
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
            else if (string.IsNullOrEmpty(activeBotUser.RequestHadnlerParameters.LastClanTagMessage)
                && (requestHandler.HandlerMenuLevel
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
            else if (string.IsNullOrEmpty(activeBotUser.RequestHadnlerParameters.AdminsKey)
                && activeBotUser.RequestHadnlerParameters.IsBotHolder is false
                && (requestHandler.HandlerMenuLevel
                     is MenuLevel.Leader1
                     or MenuLevel.DeveloperMenu2
                     or MenuLevel.LeaderAddMenu2
                     or MenuLevel.LeaderUpdateMenu2
                     or MenuLevel.LeaderDeleteMenu2
                     or MenuLevel.LeaderTgGroupCustomize2
                     or MenuLevel.LeaderDeleteRaidsMenu3
                     or MenuLevel.LeaderDeleteClanWarsMenu3))
            {
                TagsConditionChecker.SendAdminsKeyIsEmpty(activeBotUser.RequestHadnlerParameters);
            }
            //Проверка на токен авторизации создателя бота
            else if (requestHandler.HandlerMenuLevel is MenuLevel.DeveloperMenu2 && activeBotUser.RequestHadnlerParameters.IsBotHolder is false)
            {
                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
              StylingHelper.MakeItStyled($"У вас недостаточно прав для вызова этого меню, оно предназначено лишь для администратора.", UiTextStyle.Default));
            }
            //Проверка установленного для изменения клана
            else if (string.IsNullOrEmpty(activeBotUser.RequestHadnlerParameters.LastClanTagToMerge) &&
                requestHandler.HandlerMenuLevel is MenuLevel.LeaderAddMenu2 or MenuLevel.LeaderUpdateMenu2
                or MenuLevel.LeaderDeleteMenu2 or MenuLevel.LeaderTgGroupCustomize2)
            {
                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
            StylingHelper.MakeItStyled($"Для использования этих функций необходимо сначала выбрать редактируемый клан.\nДоступные кланы ➙ скопировать и отправить нужный тег.", UiTextStyle.Default));
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

    private static bool HoldSlashFuncAction(BotUser activeBotUser)
    {
        foreach (var slashFunc in _botSlashFunctions)
        {
            if (activeBotUser.RequestHadnlerParameters.Message.Text.Contains(slashFunc.Key))
            {
                if (activeBotUser.RequestHadnlerParameters.Message.Chat.Type
                         is ChatType.Channel
                         or ChatType.Group
                         or ChatType.Supergroup)
                {
                    AllSlashFunctionHandler.Handle(activeBotUser.RequestHadnlerParameters, _botSlashFunctions[slashFunc.Key]);

                    return true;
                }
                else
                {
                    ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                        StylingHelper.MakeItStyled($"Функции, начинающиеся с /group_ предназначены лишь для использования в зарегистрированных группах.\n" +
                        $"Для получения более подробной информации - ознакомьтесь с руководством.", UiTextStyle.Default));

                    return true;
                }
            }
        }

        return false;
    }

    private static bool AdminBanUserAction(BotUser activeBotUser)
    {
        var msg = activeBotUser.RequestHadnlerParameters.Message.Text;

        if (msg.ToLower().Contains("ban") && activeBotUser.RequestHadnlerParameters.IsBotHolder == true)
        {
            var messages = msg.Split(' ');

            var banList = System.IO.File.ReadAllLines(Program.BanListPath).ToList();

            if (messages[0] == "/ban")
            {
                using StreamWriter writer = new(Program.BanListPath, true);

                writer.WriteLine(messages[1]);

                activeBotUser.RequestHadnlerParameters.BotClient.SendTextMessageAsync(activeBotUser.ChatId,
                      text: StylingHelper.MakeItStyled($"Пользователь забанен.", UiTextStyle.Default),
                      parseMode: ParseMode.MarkdownV2);
            }
            if (messages[0] == "/unBan")
            {
                var answer = "Пользователя с таким chatId нет в бан листе";

                if (banList.Contains(messages[1]))
                {
                    banList.Remove(messages[1]);

                    using StreamWriter writer = new(Program.BanListPath, false);

                    foreach (var bannedUser in banList)
                    {
                        writer.WriteLine(bannedUser);
                    }

                    answer = $"Пользователя разбанен, банлист:\n\n{string.Join('\n', banList)}";
                }

                activeBotUser.RequestHadnlerParameters.BotClient.SendTextMessageAsync(activeBotUser.ChatId,
                     text: StylingHelper.MakeItStyled(answer, UiTextStyle.Name),
                     parseMode: ParseMode.MarkdownV2);
            }
            if (messages[0] == "/banList")
            {
                var answer = $"Банлист:\n\n{string.Join('\n', banList)}";

                activeBotUser.RequestHadnlerParameters.BotClient.SendTextMessageAsync(activeBotUser.ChatId,
                     text: StylingHelper.MakeItStyled(answer, UiTextStyle.Name),
                     parseMode: ParseMode.MarkdownV2);
            }

            return true;
        }

        return false;
    }

    private static bool FeedBackAction(BotUser activeBotUser)
    {
        if (activeBotUser.RequestHadnlerParameters.Message.Text.ToLower().Contains("отзыв"))
        {
            var userName = ResponseSender.DeterMineUserIdentificator(activeBotUser.RequestHadnlerParameters.Message);

            activeBotUser.RequestHadnlerParameters.BotClient.SendTextMessageAsync(Program.AdminsChatId,
                       text: StylingHelper.MakeItStyled($"ОТЗЫВ от {userName}\n[{activeBotUser.RequestHadnlerParameters.Message.Chat.Id}]\n\n" +
                       $"{activeBotUser.RequestHadnlerParameters.Message.Text}", UiTextStyle.Default),
                       parseMode: ParseMode.MarkdownV2);

            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true, StylingHelper.MakeItStyled($"Отзыв принят!", UiTextStyle.Default));

            return true;
        }

        return false;
    }

    private static bool IsAliveAction(BotUser activeBotUser)
    {
        if (activeBotUser.RequestHadnlerParameters.Message.Text.ToLower().Contains("жив"))
        {
            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true, StylingHelper.MakeItStyled($"Жив!", UiTextStyle.Default));

            return true;
        }

        return false;
    }

    private static bool TrashMessageAction(BotUser activeBotUser)
    {
        ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
            StylingHelper.MakeItStyled($"Кодовое слово или команда ﴾ {activeBotUser.RequestHadnlerParameters.Message.Text} ﴿ не предусмотрены.\n" +
            $"\nВоспользуйтесь меню (кнопка находится правой нижней части интерфейса) " +
            $"или введите корректный тег.", UiTextStyle.Default));

        return true;
    }


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
        _botUsers.First(x => x.ChatId == message.Chat.Id).RequestHadnlerParameters.IsBotHolder = true;
    }

    private static void UpsertBotHolderNewAdninKeyToMerge(Message message)
    {
        _botUsers.First(x => x.ChatId == message.Chat.Id).RequestHadnlerParameters.AdminKeyToMerge = message.Text;
    }

    private static void UpsertBotHolderNewTrackedClanTag(Message message)
    {
        _botUsers.First(x => x.ChatId == message.Chat.Id).RequestHadnlerParameters.TagToAddClan = message.Text;
    }

    private static void UpsertBotHolderNewTrackedClanChatId(Message message)
    {
        var newClanChatId = message.Text[1..];

        _botUsers.First(x => x.ChatId == message.Chat.Id).RequestHadnlerParameters.ClanChatIdToMerge = newClanChatId;
    }
    #endregion
}