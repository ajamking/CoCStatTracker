using System.Reflection;

namespace CoCStatsTrackerBot.Requests.RequestHandlers;

public static class AllRequestHandlersConstructor
{
    public static List<BaseRequestHandler> AllRequestHandlers { get; set; }

    static AllRequestHandlersConstructor()
    {
        AllRequestHandlers = new List<BaseRequestHandler>();

        var childrenTypes = Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsSubclassOf(typeof(BaseRequestHandler)));

        foreach (var item in childrenTypes)
        {
            var instance = (BaseRequestHandler)Activator.CreateInstance(item);

            AllRequestHandlers.Add(instance);
        }
    }
}
