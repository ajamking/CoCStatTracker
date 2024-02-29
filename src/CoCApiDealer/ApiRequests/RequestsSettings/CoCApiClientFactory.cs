using System.Net.Http.Headers;

namespace CoCApiDealer.ApiRequests;

public static class CoCApiClientFactory
{
    private static readonly HttpClient _client = new();

#if DEBUG
    private static readonly string _developersKey = File.ReadAllText(@"./CustomSolutionElements/ClashApiKeyPC.txt");
#else
    private static readonly string _developersKey = File.ReadAllText(@"./CustomSolutionElements/ClashApiKeyVM.txt");
#endif
    public static string DevelopersKey { get; } = _developersKey;

    public static HttpClient GetHttpClient()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DevelopersKey);

        return _client;
    }
}