namespace CoCApiDealer.ApiRequests;

public class BaseApiRequest
{
    public static HttpClient HttpClient { get; set; } = CoCApiClientFactory.GetHttpClient();
}