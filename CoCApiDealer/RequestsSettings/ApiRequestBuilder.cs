namespace CoCApiDealer.RequestsSettings;

public class ApiRequestBuilder
{
    private string _tag = string.Empty;
    private string _limit = string.Empty;

    public HttpClient HttpClient { get; }
    public AllowedRequest AllowedRequest { get; }
    public string BaseUrl { get; } = "https://api.clashofclans.com/v1";
    public string? FinalRequestUrl { get; set; }

    public ApiRequestBuilder(HttpClient authorizedClient, string tag, AllowedRequest allowedRequest, int limit = 0)
    {
        _tag = tag;

        if (limit != 0)
        {
            _limit = "?limit=" + limit.ToString();
        }

        HttpClient = authorizedClient;

        AllowedRequest = allowedRequest;
    }

    public async Task<string> CallApi()
    {
        FinalRequestUrl = @$"{BaseUrl}/{AllowedRequest.SearchBy}/%23{_tag.Substring(1)}/{AllowedRequest.LastEndPointWord}{_limit}";

        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, FinalRequestUrl);

        using HttpResponseMessage responce = HttpClient.Send(request);

        await Task.Delay(0);

        return responce.Content.ReadAsStringAsync().Result;
    }
}
