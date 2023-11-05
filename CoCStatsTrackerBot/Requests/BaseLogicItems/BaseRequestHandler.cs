using CoCStatsTrackerBot.Menu;
using System.Reflection;

namespace CoCStatsTrackerBot.Requests;

public abstract class BaseRequestHandler
{
    public static string MessageSplitToken { get; } = "answerReservedSplitter";
    public static List<BaseMenu> AllMenues { get; private set; }

    public static string DefaultNotFoundMessage = StylingHelper.MakeItStyled("Пока не обладаю запрашиваемыми сведениями.", UiTextStyle.Default);

    public static string DefaultFailerPullFromApiMessage = StylingHelper.MakeItStyled("Не удалось получить запрашиваемые сведения. Проблема на стороне CoC API", UiTextStyle.Default);

    public string Header { get; protected set; } = "/start";

    public MenuLevel HandlerMenuLevel { get; protected set; } = MenuLevel.Main0;

    static BaseRequestHandler()
    {
        AllMenues = new List<BaseMenu>();

        var childrenTypes = Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsSubclassOf(typeof(BaseMenu)));

        foreach (var item in childrenTypes)
        {
            var instance = (BaseMenu)Activator.CreateInstance(item);

            AllMenues.Add(instance);
        }
    }

    public virtual string[] SplitAnswer(string answer) => answer.Split(new[] { MessageSplitToken }, StringSplitOptions.RemoveEmptyEntries).ToArray();

    public virtual void Execute(BotUserRequestParameters parameters) { }

    public virtual void ShowKeyboard(BotUserRequestParameters parameters) => KeyboardSender.ShowKeyboard(parameters, AllMenues.First(x => x.MenuLevel == HandlerMenuLevel).Keyboard);
}
