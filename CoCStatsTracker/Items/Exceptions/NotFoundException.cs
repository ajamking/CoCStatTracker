using System;

namespace CoCStatsTracker;

public class NotFoundException : Exception
{
    public static void ThrowByPredicate(Func<bool> predicate, string msg)
    {
        if (predicate())
        {
            throw new NotFoundException(msg);
        }
    }

    public NotFoundException()
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}