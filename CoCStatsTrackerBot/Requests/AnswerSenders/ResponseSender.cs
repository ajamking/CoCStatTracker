using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot.Requests;

public static class ResponseSender
{
    public async static void SendAnswer(BotUserRequestParameters parameters, bool answerIsValid, params string[] splitedAnswer)
    {
        try
        {
            var botUserIdentitficator = DeterMineUserIdentificator(parameters.Message);

            if (answerIsValid)
            {
                foreach (var answer in splitedAnswer)
                {
                    await parameters.BotClient.SendTextMessageAsync(parameters.Message.Chat.Id,
                          text: answer,
                          parseMode: ParseMode.MarkdownV2);
                }

                WriteToConsole($"---\n{DateTime.Now}: На: \"{parameters.Message.Text}\" от {botUserIdentitficator} {parameters.Message.Chat.Id}. Ответ выдан - {answerIsValid}\n---", ConsoleColor.Green);
            }
            else
            {
                await parameters.BotClient.SendTextMessageAsync(parameters.Message.Chat.Id,
                      text: StylingHelper.MakeItStyled("Произошла внутреняя ошибка, обратитесь к администратору.", UiTextStyle.Default),
                      parseMode: ParseMode.MarkdownV2);

                WriteToConsole($"---\n{DateTime.Now}: На: \"{parameters.Message.Text}\" от {botUserIdentitficator}. Ответ выдан - {answerIsValid}\n---", ConsoleColor.DarkRed);

                Console.WriteLine($"Текст ошибки:");

                foreach (var answer in splitedAnswer)
                {
                    Console.WriteLine($"{answer}\n");
                }
            }
        }
        catch (Exception e)
        {
            WriteToConsole("Что-то не так со сформированным сообщением, ответ не был выдан.", ConsoleColor.DarkRed);
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
        _ when !string.IsNullOrEmpty(message.Chat.Username) => $"@{message.Chat.Username}",
        _ when !string.IsNullOrEmpty(message.Chat.FirstName) => message.Chat.FirstName,
        _ => message.Chat.Id.ToString(),
    };
}