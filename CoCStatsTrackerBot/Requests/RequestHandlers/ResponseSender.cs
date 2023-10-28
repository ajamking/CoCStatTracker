using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot.Requests;

public static class ResponseSender
{
    public async static void SendAnswer(RequestHadnlerParameters parameters, bool answerIsValid, params string[] splitedAnswer)
    {
        //Переписать по-другому, а то мне шото не нравится.

        if (parameters.Message.Chat.Username != string.Empty)
        {
            Console.WriteLine($"---\n{DateTime.Now}: На: \"{parameters.Message.Text}\" от {parameters.Message.Chat.Username}. Ответ выдан - {answerIsValid}\n---");
        }
        else if (parameters.Message.Chat.FirstName != string.Empty)
        {
            Console.WriteLine($"---\n{DateTime.Now}: На: \"{parameters.Message.Text}\" от {parameters.Message.Chat.FirstName}. Ответ выдан - {answerIsValid}\n---");
        }
        else
        {
            Console.WriteLine($"---\n{DateTime.Now}: На: \"{parameters.Message.Text}\" от {parameters.Message.Chat.Id}. Ответ выдан - {answerIsValid}\n---");
        }

        foreach (var answer in splitedAnswer)
        {
            await parameters.BotClient.SendTextMessageAsync(parameters.Message.Chat.Id,
                  text: answer,
                  parseMode: ParseMode.MarkdownV2);
        }
    }
}
