using CoCStatsTrackerBot.Menu;
using System.Reflection;

namespace CoCStatsTrackerBot.Requests;

public abstract class BaseRequestHandler
{
    public static string MessageSplitToken { get; } = "answerReservedSplitter";
    public static List<BaseMenu> AllMenues { get; private set; }

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

    public virtual void Execute(RequestHadnlerParameters parameters) { }

    public virtual void ShowKeyboard(RequestHadnlerParameters parameters) => KeyboardSender.ShowKeyboard(parameters, AllMenues.First(x => x.MenuLevel == HandlerMenuLevel).Keyboard);
}
