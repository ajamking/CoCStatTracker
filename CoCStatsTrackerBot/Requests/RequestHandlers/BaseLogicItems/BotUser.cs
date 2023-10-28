using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class BotUser
{
    public long ChatId { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string LastTagMessage { get; set; }
    public MenuLevels LastMenuLevel { get; set; } = MenuLevels.Main0;
    public bool IsAdmin { get; set; } = false;
}
