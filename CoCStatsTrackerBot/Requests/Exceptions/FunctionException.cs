namespace CoCStatsTrackerBot.Exceptions;

public class FunctionException : Exception
{
    public FunctionException(Exception e) : base()
    {
        Console.WriteLine("FunctionException" + e.Message + "\n" + e.StackTrace);
    }

}
