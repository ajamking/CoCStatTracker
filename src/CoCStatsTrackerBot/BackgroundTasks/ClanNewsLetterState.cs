namespace CoCStatsTrackerBot;

public class ClanNewsLetterState
{
    public string Tag { get; set; }
    public string Name { get; set; }
    public string TelegramsChatId { get; set; }

    public bool RaidStartNewsLetterOn { get; set; }
    public bool RaidEndNewsLetterOn { get; set; }

    public double RaidTimeToMessageBeforeEnd { get; set; } = 0.0;
    public DateTime RaidStartedOn { get; set; } = DateTime.MinValue;
    public bool RaidIsStartMessageSent { get; set; } = false;
    public bool RaidIsCustomTimeMessageSent { get; set; } = false;
    public bool RaidIsEndMessageSent { get; set; } = false;

    public bool WarStartNewsLetterOn { get; set; }
    public bool WarEndNewsLetterOn { get; set; }

    public double WarTimeToMessageBeforeEnd { get; set; } = 0.0;
    public DateTime WarStartedOn { get; set; } = DateTime.MinValue;
    public bool WarIsStartMessageSent { get; set; } = false;
    public bool WarIsCustomTimeMessageSent { get; set; } = false;
    public bool WarIsEndMessageSent { get; set; } = false;

    public ClanNewsLetterState(string tag, string name, string telegramsChatId, int usersTimeBeforeRaidEnd, int usersTimeBeforeWarEnd)
    {
        Tag = tag;
        Name = name;
        TelegramsChatId = telegramsChatId;
        RaidTimeToMessageBeforeEnd = usersTimeBeforeRaidEnd;
        WarTimeToMessageBeforeEnd = usersTimeBeforeWarEnd;
    }
}