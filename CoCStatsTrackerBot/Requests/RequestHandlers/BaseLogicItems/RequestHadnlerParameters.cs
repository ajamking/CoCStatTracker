using Domain.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CoCStatsTrackerBot.Requests;

public class RequestHadnlerParameters
{
    public ITelegramBotClient BotClient { get; init; }
    public Message Message { get; init; }
    public string? LastTagMessage { get; set; }
    public int EntriesCount { get; set; } = 0;
    public UnitType UnitType { get; set; } = 0;
    public DistrictType DistrictType { get; set; } = 0;

    public RequestHadnlerParameters(ITelegramBotClient botClient, Message message, string lastTagMessage)
    {
        BotClient = botClient;
        Message = message;
        LastTagMessage = lastTagMessage;
    }
}
