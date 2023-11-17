namespace CoCApiDealer.ApiRequests;

public class ApiInMaintenanceException : Exception
{
    public static void ThrowByPredicate(Func<bool> predicate, string msg)
    {
        if (predicate())
        {
            throw new ApiInMaintenanceException(msg);
        }
    }

    public ApiInMaintenanceException()
    {
    }

    public ApiInMaintenanceException(string message) : base(message)
    {
    }

    public ApiInMaintenanceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}