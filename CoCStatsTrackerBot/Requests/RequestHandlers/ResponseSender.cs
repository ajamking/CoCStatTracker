using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot.Requests;

public static class ResponseSender
{
    public async static void SendAnswer(RequestHadnlerParameters parameters, bool answerIsValid, params string[] splitedAnswer)
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
                  text: StylingHelper.MakeItStyled("", UiTextStyle.Default),
                  parseMode: ParseMode.MarkdownV2);

        }
    }
    

    private static string DeterMineUserIdentificator(Message message)
    {
        var botUserIdentitficator = "";

        if (message.Chat.Username != null)
        {
            botUserIdentitficator = message.Chat.Username;
        }
        else if (message.Chat.FirstName != null)
        {
            botUserIdentitficator = message.Chat.FirstName;
        }
        else
        {
            botUserIdentitficator = message.Chat.Id.ToString();
        }

        return botUserIdentitficator;
    }
}
