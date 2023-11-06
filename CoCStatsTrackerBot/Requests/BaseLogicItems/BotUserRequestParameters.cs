using Domain.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CoCStatsTrackerBot.Requests;

public class BotUserRequestParameters
{
    public bool IsBotHolder { get; set; }
    public string AdminsKey { get; set; }

    public ITelegramBotClient BotClient { get; set; }
    public Message Message { get; set; }

    public string LastClanTagMessage { get; set; }
    public string LastMemberTagMessage { get; set; }

    public string TagToAddClan { get; set; }
    public string LastClanTagToMerge { get; set; }
    public string AdminKeyToMerge { get; set; }

    public int EntriesCount { get; set; } = 0;
    public UnitType UnitType { get; set; } = 0;
    public ADistrictType DistrictType { get; set; } = 0;

    public BotUserRequestParameters(ITelegramBotClient botClient, Message message, string lastClanTagMessage, string lastMemberTagMessage)
    {
        BotClient = botClient;
        Message = message;
        LastClanTagMessage = lastClanTagMessage;
        LastMemberTagMessage = lastMemberTagMessage;
    }
}