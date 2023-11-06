namespace CoCStatsTrackerBot.Exceptions;

public class RequestHandlerException : Exception
{
    public RequestHandlerException(Exception e) : base()
    {
        Console.WriteLine("RequestHandlerException" + e.Message + "\n" + e.StackTrace);
    }
}
