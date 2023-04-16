using CoCApiDealer.RequestsSettings;

namespace CoCApiDealer.ApiRequests;

public class BaseApiRequest
{
    public static HttpClient _httpClient = new CoCApiClientFactory().GetHttpClient();
}
