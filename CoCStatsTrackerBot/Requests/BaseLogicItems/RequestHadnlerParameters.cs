using Domain.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CoCStatsTrackerBot.Requests;

public class RequestHadnlerParameters
{
    public ITelegramBotClient BotClient { get; set; }
    public Message Message { get; set; }
    public string LastClanTagMessage { get; set; }
    public string LastMemberTagMessage { get; set; }
    public int EntriesCount { get; set; } = 0;
    public UnitType UnitType { get; set; } = 0;
    public DistrictType DistrictType { get; set; } = 0;

    public RequestHadnlerParameters(ITelegramBotClient botClient, Message message, string lastClanTagMessage, string lastMemberTagMessage)
    {
        BotClient = botClient;
        Message = message;
        LastClanTagMessage = lastClanTagMessage;
        LastMemberTagMessage = lastMemberTagMessage;
    }
}
