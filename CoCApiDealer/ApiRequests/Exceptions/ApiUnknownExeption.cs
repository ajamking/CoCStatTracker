namespace CoCApiDealer.ApiRequests;

public class ApiUnknownExeption : Exception
{
    public static void ThrowByPredicate(Func<bool> predicate, string msg)
    {
        if (predicate())
        {
            throw new ApiNullOrEmtyResponseException(msg);
        }
    }

    public ApiUnknownExeption()
    {
    }

    public ApiUnknownExeption(string message) : base(message)
    {
    }

    public ApiUnknownExeption(string message, Exception innerException) : base(message, innerException)
    {
    }
}