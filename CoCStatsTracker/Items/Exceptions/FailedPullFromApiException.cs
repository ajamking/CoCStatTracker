using System;

namespace CoCStatsTracker.Items.Exceptions;

public class FailedPullFromApiException : Exception
{
    public static void ThrowByPredicate(Func<bool> predicate, string msg)
    {
        if (predicate())
        {
            throw new FailedPullFromApiException(msg);
        }
    }

    public FailedPullFromApiException()
    {

    }

    public FailedPullFromApiException(string message) : base(message)
    {
    }

    public FailedPullFromApiException(string message, Exception innerException) : base(message, innerException)
    {
    }
}