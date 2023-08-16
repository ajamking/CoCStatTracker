namespace CoCApiDealer.ApiRequests;
internal class ApiErrorException : Exception
{
    public ApiErrorException(Exception ex) : base()
    {
       // Переделать этот пиздец
        Console.WriteLine(ex.StackTrace + " an error has occured. Exeption text:" + ex.Message + "\n");
    }
}
