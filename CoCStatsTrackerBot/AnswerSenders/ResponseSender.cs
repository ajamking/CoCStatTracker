using CoCStatsTrackerBot.Requests;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CoCStatsTrackerBot;

public static class ResponseSender
{
    public async static void SendAnswer(BotUserRequestParameters parameters, bool answerIsValid, params string[] splitedAnswer)
    {
        try
        {
            var botUserIdentificator = DeterMineUserIdentificator(parameters.Message);

            if (answerIsValid)
            {
                foreach (var answer in splitedAnswer)
                {
                    await parameters.BotClient.SendTextMessageAsync(parameters.Message.Chat.Id,
                          text: answer,
                          parseMode: ParseMode.MarkdownV2);
                }

                WriteToConsole($"---\n{DateTime.Now}: На: \"{parameters.Message.Text}\" от {botUserIdentificator}. Ответ выдан - {answerIsValid}\n---", ConsoleColor.Green);
            }
            else
            {
                await parameters.BotClient.SendTextMessageAsync(parameters.Message.Chat.Id,
                      text: StylingHelper.MakeItStyled("Произошла внутреняя ошибка, обратитесь к администратору.", UiTextStyle.Default),
                parseMode: ParseMode.MarkdownV2);

                var tempException = new Exception();

                tempException.LogException(parameters.Message.Chat.Username, parameters.Message.Chat.Id, parameters.Message.Text, "Что-то пошло не так, был выдан ответ-заглушка.");

                WriteToConsole($"---\n{DateTime.Now}: На: \"{parameters.Message.Text}\" от {botUserIdentificator}. Ответ выдан - {answerIsValid}\n---", ConsoleColor.DarkRed);

                Console.WriteLine($"Текст ошибки:");

                foreach (var answer in splitedAnswer)
                {
                    Console.WriteLine($"{answer}\n");
                }
            }
        }
        catch (Telegram.Bot.Exceptions.ApiRequestException exception)
        {
            HandleBotApiExceptions(exception, parameters);

            return;
        }
        catch (Exception e)
        {
            var customMessage = "Что-то не так со сформированным сообщением, ответ не был выдан.";

            e.LogException(parameters.Message.Chat.Username, parameters.Message.Chat.Id, parameters.Message.Text, customMessage);

            WriteToConsole($"{customMessage} {e.Message}", ConsoleColor.DarkRed);

            return;
        }
    }

    private static void WriteToConsole(string message, ConsoleColor colour)
    {
        Console.ForegroundColor = colour;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static string DeterMineUserIdentificator(Message message) => message switch
    {
        _ when !string.IsNullOrEmpty(message.Chat.Username) => $"@{message.Chat.Username} {message.Chat.Id}",
        _ when !string.IsNullOrEmpty(message.Chat.FirstName) => $"@{message.Chat.FirstName} {message.Chat.Id}",
        _ => message.Chat.Id.ToString(),
    };

    public static async void HandleBotApiExceptions(Telegram.Bot.Exceptions.ApiRequestException exception, BotUserRequestParameters parameters)
    {
        switch (exception.ErrorCode)
        {
            case 429:
                {
                    exception.LogException(parameters.Message.Chat.Username, parameters.Message.Chat.Id, parameters.Message.Text);

                    WriteToConsole($"Слишком много сообщений от: {parameters.Message.Chat.Username} {parameters.Message.Chat.Id}" + exception.Message, ConsoleColor.DarkRed);

                    using StreamWriter writer = new(Program.BanListPath, true);

                    writer.WriteLine(parameters.Message.Chat.Id);

                    await parameters.BotClient.SendTextMessageAsync(parameters.Message.Chat.Id,
                         text: StylingHelper.MakeItStyled("Вы злонамеренно отправили слишком большое количество запросов и были заблокированы ботом.", UiTextStyle.Default),
                         parseMode: ParseMode.MarkdownV2);

                    break;
                }
            default:
                {
                    exception.LogException(parameters.Message.Chat.Username, parameters.Message.Chat.Id, parameters.Message.Text);

                    WriteToConsole($"На запрос от: {parameters.Message.Chat.Username} {parameters.Message.Chat.Id} HandleBotApiExceptions словил исключение: {exception.Message}", ConsoleColor.DarkRed);

                    await parameters.BotClient.SendTextMessageAsync(parameters.Message.Chat.Id,
                        text: StylingHelper.MakeItStyled("В работе бота возникли помехи, скорее всего проблема с сервером телеграмма. Приходите позже.", UiTextStyle.Default),
                        parseMode: ParseMode.MarkdownV2);
                }
                break;
        }
    }
}