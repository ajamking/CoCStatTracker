using CoCStatsTrackerBot.Menu;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot.Requests;

public abstract class BaseRequestHandler
{
    public string MessageSplitToken { get; } = "answerReservedSplitter";

    public string Header { get; protected set; } = "/start";

    public MenuLevels HandlerMenuLevel { get; protected set; } = MenuLevels.Main0;

    public static List<BaseMenu> AllMenues { get; private set; }

    public BaseRequestHandler()
    {
        AllMenues = new List<BaseMenu>()
        {
        new MainMenu0(),
        new MemberMenu1(),
        new PlayerInfo2(),
        new ClanInfo2(),
        new ClanCurrentWarInfo2(),
        new ClanCurrentRaidInfo2(),
        new PlayerWarStatistics3(),
        new PlayerRaidStatistics3(),
        new PlayerArmy3(),
        new ClanWarHistory3(),
        new ClanRaidHistory3(),
        new ClanCurrentDistrictStatistics3(),
        };
    }

    public virtual string[] SplitAnswer(string answer) => answer.Split(new[] { MessageSplitToken }, StringSplitOptions.RemoveEmptyEntries).ToArray();

    public virtual void Execute(RequestHadnlerParameters parameters) { }

    public virtual void ShowKeyboard(RequestHadnlerParameters parameters) => KeyboardSender.ShowKeyboard(parameters, AllMenues.First(x => x.MenuLevel == HandlerMenuLevel).Keyboard);
}
