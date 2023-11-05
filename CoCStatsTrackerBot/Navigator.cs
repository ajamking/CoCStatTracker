using CoCStatsTrackerBot.Helpers;
using CoCStatsTrackerBot.Menu;
using CoCStatsTrackerBot.Requests;
using CoCStatsTrackerBot.Requests.RequestHandlers;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CoCStatsTrackerBot;

public static class Navigator
{
    private static readonly string _botHolderToken = System.IO.File.ReadAllText(@"./../../../../CustomSolutionElements/MyPersonalKey.txt");
    private static Regex _tagRegex { get; set; } = new Regex(@"^#(\w{6,9})$");

    private static List<BaseRequestHandler> _allRequestHandlers = AllRequestHandlersConstructor.AllRequestHandlers;

    private static List<BotUser> _botUsers = new List<BotUser>();

    public async static void HandleMessage(ITelegramBotClient botClient, Message message)
    {
        TryAddBotUser(botClient, message);

        var activeBotUser = _botUsers.First(x => x.ChatId == message.Chat.Id);

        UpdateUsersRHParametersMesssage(activeBotUser, message);

        if (activeBotUser.RequestHadnlerParameters.Message.Text == _botHolderToken)
        {
            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                                StylingHelper.MakeItStyled($"Вы определены как владелец бота.", UiTextStyle.Default));

            UpsertBotHolderProperty(activeBotUser.RequestHadnlerParameters.Message);
        }
        else if (activeBotUser.RequestHadnlerParameters.Message.Text.ToLower() == "назад")
        {
            switch (activeBotUser.CurrentMenuLevel)
            {
                case MenuLevel.PlayerInfo2 or MenuLevel.ClanInfo2
                     or MenuLevel.CurrentWarInfo2 or MenuLevel.CurrentRaidInfo2:
                    {
                        _allRequestHandlers
                            .First(x => x.HandlerMenuLevel == MenuLevel.Member1)
                            .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                        activeBotUser.CurrentMenuLevel = MenuLevel.Member1;

                        return;
                    }
                case MenuLevel.PlayerWarStatistics3 or MenuLevel.PlayerRaidStatistics3
                     or MenuLevel.PlayerArmy3:
                    {
                        _allRequestHandlers
                           .First(x => x.HandlerMenuLevel == MenuLevel.PlayerInfo2)
                           .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                        activeBotUser.CurrentMenuLevel = MenuLevel.PlayerInfo2;

                        return;
                    }
                case MenuLevel.ClanWarsHistory3 or MenuLevel.ClanRaidsHistory3:
                    {
                        _allRequestHandlers
                           .First(x => x.HandlerMenuLevel == MenuLevel.ClanInfo2)
                           .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                        activeBotUser.CurrentMenuLevel = MenuLevel.ClanInfo2;

                        return;
                    }
                case MenuLevel.CurrentDistrictStatistics3:
                    {

                        _allRequestHandlers
                         .First(x => x.HandlerMenuLevel == MenuLevel.CurrentRaidInfo2)
                         .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                        activeBotUser.CurrentMenuLevel = MenuLevel.CurrentRaidInfo2;

                        return;
                    }
                case MenuLevel.DeveloperMenu2 or MenuLevel.LeaderAddMenu2
                     or MenuLevel.LeaderUpdateMenu2 or MenuLevel.LeaderDeleteMenu2:
                    {
                        _allRequestHandlers
                        .First(x => x.HandlerMenuLevel == MenuLevel.Leader1)
                        .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                        activeBotUser.CurrentMenuLevel = MenuLevel.Leader1;

                        return;
                    }
                case MenuLevel.LeaderDeleteRaidsMenu3 or MenuLevel.LeaderDeleteClanWarsMenu3:
                    {
                        _allRequestHandlers
                        .First(x => x.HandlerMenuLevel == MenuLevel.LeaderDeleteMenu2)
                        .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                        activeBotUser.CurrentMenuLevel = MenuLevel.LeaderDeleteMenu2;

                        return;
                    }
                default:
                    {
                        _allRequestHandlers
                         .First(x => x.HandlerMenuLevel == MenuLevel.Main0)
                         .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                        activeBotUser.CurrentMenuLevel = MenuLevel.Main0;

                        return;
                    }
            }
        }
        else if (activeBotUser.RequestHadnlerParameters.Message.Text.CheckClanWithAdminsKeyExistsInDb())
        {
            UpsertLastAdminsKey(activeBotUser.RequestHadnlerParameters.Message);

            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                            StylingHelper.MakeItStyled($"Токен авторизации задан корректно, выберите пункт из меню.", UiTextStyle.Default));
        }
        else if (activeBotUser.RequestHadnlerParameters.Message.Text.Contains('#') && (activeBotUser.CurrentMenuLevel is MenuLevel.Leader1 or MenuLevel.DeveloperMenu2 or MenuLevel.LeaderAddMenu2 or MenuLevel.LeaderUpdateMenu2 or MenuLevel.LeaderDeleteMenu2 or MenuLevel.LeaderDeleteRaidsMenu3 or MenuLevel.LeaderDeleteClanWarsMenu3))
        {
            if (TagsConditionChecker.CheckClanIsAllowedToMerge(activeBotUser.RequestHadnlerParameters))
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
        }
        else if (activeBotUser.RequestHadnlerParameters.Message.Text.Contains('#'))
        {
            switch (activeBotUser.RequestHadnlerParameters.Message.Text)
            {
                case string msg when _tagRegex.IsMatch(msg):
                    {
                        if (TagsConditionChecker.CheckClanExistInDb(activeBotUser.RequestHadnlerParameters))
                        {
                            UpsertLastClanTagMessage(message);

                            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                                StylingHelper.MakeItStyled($"Тег задан корректно! У меня есть информация об этом клане.", UiTextStyle.Default));

                            return;
                        }
                        else if (TagsConditionChecker.CheckMemberExistInDb(activeBotUser.RequestHadnlerParameters))
                        {
                            UpsertLastMemberTagMessage(message);

                            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                                StylingHelper.MakeItStyled($"Тег задан корректно! У меня есть информация об этом игроке.", UiTextStyle.Default));

                            return;
                        }
                        else
                        {
                            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                                StylingHelper.MakeItStyled($"Тег был задан в корректной форме, но ни игрок ни клан с таким тегом" +
                                $" не отслеживаются. Попробуйте ввести другой.", UiTextStyle.Default));

                            return;
                        }
                    }
                default:
                    {
                        ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                          StylingHelper.MakeItStyled($"Тег не прошел проверку.\r\nЗадайте тег в корректной форме или выберите другуо опцию из меню.", UiTextStyle.Default));

                        return;
                    }
            }
        }
        else if (_allRequestHandlers.Any(x => x.Header == activeBotUser.RequestHadnlerParameters.Message.Text))
        {
            var requestHandler = _allRequestHandlers.First(x => x.Header == activeBotUser.RequestHadnlerParameters.Message.Text);

            if (string.IsNullOrEmpty(activeBotUser.RequestHadnlerParameters.LastMemberTagMessage) && (requestHandler.HandlerMenuLevel
                is MenuLevel.PlayerInfo2
                or MenuLevel.PlayerWarStatistics3
                or MenuLevel.PlayerRaidStatistics3
                or MenuLevel.PlayerArmy3))
            {
                TagsConditionChecker.SendMemberTagMessageIsEmpty(activeBotUser.RequestHadnlerParameters);
            }
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
            else if (string.IsNullOrEmpty(activeBotUser.RequestHadnlerParameters.AdminsKey) && (
                     activeBotUser.RequestHadnlerParameters.Message.Text is "Интерфейс главы клана" ||
                     activeBotUser.CurrentMenuLevel
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
            else if (activeBotUser.RequestHadnlerParameters.Message.Text is "Меню создателя" && activeBotUser.IsBotHolder is false)
            {
                ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
              StylingHelper.MakeItStyled($"У вас недостаточно прав для вызова этого меню.", UiTextStyle.Default));
            }
            else
            {
                requestHandler.Execute(activeBotUser.RequestHadnlerParameters);

                UpdateUserMenuLevels(activeBotUser, requestHandler);
            }
        }
        else
        {
            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                StylingHelper.MakeItStyled($"Вы сказали {activeBotUser.RequestHadnlerParameters.Message.Text}, но я еще не знаю таких сложных вещей.🥺 \r\n" +
                $"Выберите что-то из меню или введите корректный тег игрока или кална.😁", UiTextStyle.Default));
        }
    }




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
}