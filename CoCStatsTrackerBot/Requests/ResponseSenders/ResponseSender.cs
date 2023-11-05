using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot.Requests;

public static class ResponseSender
{
    public async static void SendAnswer(BotUserRequestParameters parameters, bool answerIsValid, params string[] splitedAnswer)
    {
        var botUserIdentitficator = DeterMineUserIdentificator(parameters.Message);

        if (answerIsValid)
        {
            Console.WriteLine($"---\n{DateTime.Now}: На: \"{parameters.Message.Text}\" от {botUserIdentitficator}. Ответ выдан - {answerIsValid}\n---");

            foreach (var answer in splitedAnswer)
            {
                await parameters.BotClient.SendTextMessageAsync(parameters.Message.Chat.Id,
                      text: answer,
                      parseMode: ParseMode.MarkdownV2);
            }
        }
        else
        {
            Console.WriteLine($"---\n{DateTime.Now}: На: \"{parameters.Message.Text}\" от {botUserIdentitficator}. Ответ выдан - {answerIsValid}\n---");

            Console.WriteLine($"Текст ошибки:");

            foreach (var answer in splitedAnswer)
            {
                Console.WriteLine($"{answer}\n");
            }

            await parameters.BotClient.SendTextMessageAsync(parameters.Message.Chat.Id,
                  text: StylingHelper.MakeItStyled("Произошла внутреняя ошибка, обратитесь к администратору.", UiTextStyle.Default),
                  parseMode: ParseMode.MarkdownV2);
        }
    }

    private static string DeterMineUserIdentificator(Message message) => message switch
    {
        _ when !string.IsNullOrEmpty(message.Chat.Username) => message.Chat.Username,
        _ when !string.IsNullOrEmpty(message.Chat.FirstName) => message.Chat.FirstName,
        _ => message.Chat.Id.ToString(),
    };
}