namespace CoCApiDealer.ApiRequests;

public class ApiNullOrEmtyResponseException : Exception
{
    public static void ThrowByPredicate(Func<bool> predicate, string msg)
    {
        if (predicate())
        {
            throw new ApiNullOrEmtyResponseException(msg);
        }
    }

    public ApiNullOrEmtyResponseException()
    {
    }

    public ApiNullOrEmtyResponseException(string message) : base(message)
    {
    }

    public ApiNullOrEmtyResponseException(string message, Exception innerException) : base(message, innerException)
    {
    }
}