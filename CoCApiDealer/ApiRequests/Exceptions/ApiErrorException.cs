namespace CoCApiDealer.ApiRequests;
internal class ApiErrorException : Exception
{
    public ApiErrorException(string msg) : base(msg)
    {

    }
}
