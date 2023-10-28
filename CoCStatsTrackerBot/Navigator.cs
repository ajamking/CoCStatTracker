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
        TryAddBotUser(message);

        var activeBotUser = BotUsers.First(x => x.ChatId == message.Chat.Id);

        if (message.Text?.ToLower() == "назад")
        {
            switch (activeBotUser.LastMenuLevel)
            {
                case MenuLevels.Member1:
                    {
                        AllRequestHandlers
                            .First(x => x.HandlerMenuLevel == MenuLevels.Main0)
                            .ShowKeyboard(new RequestHadnlerParameters(botClient, message, activeBotUser.LastTagMessage));

                        activeBotUser.LastMenuLevel = MenuLevels.Main0;

                        return;
                    }
                case MenuLevels.PlayerInfo2 or MenuLevels.ClanInfo2 or MenuLevels.CurrentWarInfo2 or
                     MenuLevels.CurrentRaidInfo2:
                    {
                        AllRequestHandlers
                            .First(x => x.HandlerMenuLevel == MenuLevels.Member1)
                            .ShowKeyboard(new RequestHadnlerParameters(botClient, message, activeBotUser.LastTagMessage));

                        activeBotUser.LastMenuLevel = MenuLevels.Member1;

                        return;
                    }
                case MenuLevels.PlayerWarStatistics3 or MenuLevels.PlayerRaidStatistics3 or MenuLevels.PlayerArmy3:
                    {
                        AllRequestHandlers
                           .First(x => x.HandlerMenuLevel == MenuLevels.PlayerInfo2)
                           .ShowKeyboard(new RequestHadnlerParameters(botClient, message, activeBotUser.LastTagMessage));

                        activeBotUser.LastMenuLevel = MenuLevels.PlayerInfo2;

                        return;
                    }
                case MenuLevels.ClanWarsHistory3 or MenuLevels.ClanRaidsHistory3:
                    {
                        AllRequestHandlers
                           .First(x => x.HandlerMenuLevel == MenuLevels.ClanInfo2)
                           .ShowKeyboard(new RequestHadnlerParameters(botClient, message, activeBotUser.LastTagMessage));

                        activeBotUser.LastMenuLevel = MenuLevels.ClanInfo2;

                        return;
                    }
                case MenuLevels.CurrentDistrictStatistics3:
                    {

                        AllRequestHandlers
                         .First(x => x.HandlerMenuLevel == MenuLevels.CurrentRaidInfo2)
                         .ShowKeyboard(new RequestHadnlerParameters(botClient, message, activeBotUser.LastTagMessage));

                        activeBotUser.LastMenuLevel = MenuLevels.CurrentRaidInfo2;

                        return;
                    }
                default:
                    {
                        AllRequestHandlers
                         .First(x => x.HandlerMenuLevel == MenuLevels.Main0)
                         .ShowKeyboard(new RequestHadnlerParameters(botClient, message, activeBotUser.LastTagMessage));

                        activeBotUser.LastMenuLevel = MenuLevels.Main0;

                        return;
                    }
            }
        }

        if (message.Text.Contains('#'))
        {
            switch (message.Text)
            {
                case string msg when TagRegex.IsMatch(msg):
                    {
                        UpsertLastTagMessage(message);

                        await botClient.SendTextMessageAsync(message.Chat.Id, text: "Тег задан в корректной форме!");

                        return;
                    }
                default:
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, text: "Тег не прошел проверку. " +
                            "Задайте тег в корректной форме или выберите другуо опцию из меню.");

                        return;
                    }
            }
        }

        if (AllRequestHandlers.Any(x => x.Header == message.Text))
        {
            var requestHandler = AllRequestHandlers.First(x => x.Header == message.Text);

            var requestParameters = new RequestHadnlerParameters(botClient, message, activeBotUser.LastTagMessage);

            activeBotUser.LastMenuLevel = requestHandler.HandlerMenuLevel;

            requestHandler.ShowKeyboard(requestParameters);

            requestHandler.Execute(requestParameters);
        }
        else
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
                  text: $"Вы сказали {message.Text}, но я еще не знаю таких сложных вещей. 🥺\n" +
                  $"Выберите что-то из меню или введите корректный тег игрока/клана 😁");

        }
    }

    public static void TryAddBotUser(Message message)
    {
        if (!BotUsers.Any(x => x.ChatId == message.Chat.Id))
        {
            BotUsers.Add(new BotUser()
            {
                ChatId = message.Chat.Id,
                FirstName = message.Chat.FirstName,
                Username = message.Chat.Username,
            });
        }
    }

    public static void UpsertLastTagMessage(Message message)
    {
        BotUsers.First(x => x.ChatId == message.Chat.Id).LastTagMessage = message.Text;
    }
}
