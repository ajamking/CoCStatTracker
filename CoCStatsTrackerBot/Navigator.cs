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
    public static List<BaseRequestHandler> AllRequestHandlers = AllRequestHandlersConstructor.AllRequestHandlers;

    public static List<BotUser> BotUsers = new List<BotUser>();

    public static Regex TagRegex { get; set; } = new Regex(@"^#(\w{6,9})$");

    public async static Task HandleMessage(ITelegramBotClient botClient, Message message)
    {
        TryAddBotUser(botClient, message);

        var activeBotUser = BotUsers.First(x => x.ChatId == message.Chat.Id);

        UpdateUsersRHParametersMesssage(activeBotUser, message);

        if (activeBotUser.RequestHadnlerParameters.Message.Text.ToLower() == "назад")
        {
            switch (activeBotUser.CurrentMenuLevel)
            {
                case MenuLevel.Member1:
                    {
                        AllRequestHandlers
                            .First(x => x.HandlerMenuLevel == MenuLevel.Main0)
                            .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                        activeBotUser.CurrentMenuLevel = MenuLevel.Main0;

                        return;
                    }
                case MenuLevel.PlayerInfo2 or MenuLevel.ClanInfo2 or MenuLevel.CurrentWarInfo2 or
                     MenuLevel.CurrentRaidInfo2:
                    {
                        AllRequestHandlers
                            .First(x => x.HandlerMenuLevel == MenuLevel.Member1)
                            .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                        activeBotUser.CurrentMenuLevel = MenuLevel.Member1;

                        return;
                    }
                case MenuLevel.PlayerWarStatistics3 or MenuLevel.PlayerRaidStatistics3 or MenuLevel.PlayerArmy3:
                    {
                        AllRequestHandlers
                           .First(x => x.HandlerMenuLevel == MenuLevel.PlayerInfo2)
                           .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                        activeBotUser.CurrentMenuLevel = MenuLevel.PlayerInfo2;

                        return;
                    }
                case MenuLevel.ClanWarsHistory3 or MenuLevel.ClanRaidsHistory3:
                    {
                        AllRequestHandlers
                           .First(x => x.HandlerMenuLevel == MenuLevel.ClanInfo2)
                           .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                        activeBotUser.CurrentMenuLevel = MenuLevel.ClanInfo2;

                        return;
                    }
                case MenuLevel.CurrentDistrictStatistics3:
                    {

                        AllRequestHandlers
                         .First(x => x.HandlerMenuLevel == MenuLevel.CurrentRaidInfo2)
                         .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                        activeBotUser.CurrentMenuLevel = MenuLevel.CurrentRaidInfo2;

                        return;
                    }
                default:
                    {
                        AllRequestHandlers
                         .First(x => x.HandlerMenuLevel == MenuLevel.Main0)
                         .ShowKeyboard(activeBotUser.RequestHadnlerParameters);

                        activeBotUser.CurrentMenuLevel = MenuLevel.Main0;

                        return;
                    }
            }
        }

        if (activeBotUser.RequestHadnlerParameters.Message.Text.Contains('#'))
        {
            switch (activeBotUser.RequestHadnlerParameters.Message.Text)
            {
                case string msg when TagRegex.IsMatch(msg):
                    {
                        if (TagsConditionChecker.CheckClanExistInDb(activeBotUser.RequestHadnlerParameters))
                        {
                            UpsertLastClanTagMessage(message);

                            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true, StylingHelper.MakeItStyled($"Тег клана задан в корректной форме!", UiTextStyle.Default));

                            return;
                        }
                        else if (TagsConditionChecker.CheckMemberExistInDb(activeBotUser.RequestHadnlerParameters))
                        {
                            UpsertLastMemberTagMessage(message);

                            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true, StylingHelper.MakeItStyled($"Тег игрока задан в корректной форме!", UiTextStyle.Default));

                            return;
                        }
                        else
                        {
                            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true, StylingHelper.MakeItStyled($"Тег был задан в корректной форме, но ни игрок ни клан с таким тегом" +
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

        if (AllRequestHandlers.Any(x => x.Header == activeBotUser.RequestHadnlerParameters.Message.Text))
        {
            var requestHandler = AllRequestHandlers.First(x => x.Header == activeBotUser.RequestHadnlerParameters.Message.Text);

            if (activeBotUser.RequestHadnlerParameters.LastMemberTagMessage is null && (requestHandler.HandlerMenuLevel is
                 MenuLevel.PlayerInfo2 or
                 MenuLevel.PlayerWarStatistics3 or
                 MenuLevel.PlayerRaidStatistics3 or
                 MenuLevel.PlayerArmy3))
            {
                TagsConditionChecker.SendMemberTagMessageIsEmpty(activeBotUser.RequestHadnlerParameters);
            }
            else if (activeBotUser.RequestHadnlerParameters.LastClanTagMessage is null && (requestHandler.HandlerMenuLevel is
                  MenuLevel.ClanInfo2 or
                  MenuLevel.ClanWarsHistory3 or
                  MenuLevel.ClanRaidsHistory3 or
                  MenuLevel.CurrentDistrictStatistics3 or
                  MenuLevel.CurrentWarInfo2 or
                  MenuLevel.CurrentRaidInfo2))
            {
                TagsConditionChecker.SendClanTagMessageIsEmpty(activeBotUser.RequestHadnlerParameters);
            }
            else
            {
                requestHandler.Execute(activeBotUser.RequestHadnlerParameters);

                ChangUserMenuLevels(activeBotUser, requestHandler);
            }
        }
        else
        {
            ResponseSender.SendAnswer(activeBotUser.RequestHadnlerParameters, true,
                StylingHelper.MakeItStyled($"Вы сказали {message.Text}, но я еще не знаю таких сложных вещей.🥺 \r\n" +
                $"Выберите что-то из меню или введите корректный тег игрока или кална.😁", UiTextStyle.Default));
        }
    }

    private static void TryAddBotUser(ITelegramBotClient telegramBotClient, Message message)
    {
        if (!BotUsers.Any(x => x.ChatId == message.Chat.Id))
        {
            BotUsers.Add(new BotUser(telegramBotClient, message));
        }
    }

    private static void UpsertLastClanTagMessage(Message message)
    {
        BotUsers.First(x => x.ChatId == message.Chat.Id).RequestHadnlerParameters.LastClanTagMessage = message.Text;
    }

    private static void UpsertLastMemberTagMessage(Message message)
    {
        BotUsers.First(x => x.ChatId == message.Chat.Id).RequestHadnlerParameters.LastMemberTagMessage = message.Text;
    }

    private static void UpdateUsersRHParametersMesssage(BotUser activeBotUser, Message message)
    {
        activeBotUser.RequestHadnlerParameters.Message = message;
    }

    private static void ChangUserMenuLevels(BotUser activeBotUser, BaseRequestHandler requestHandler)
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
}
