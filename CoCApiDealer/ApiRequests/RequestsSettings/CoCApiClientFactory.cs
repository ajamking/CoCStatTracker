using System.Net.Http.Headers;

namespace CoCApiDealer.ApiRequests;

public static class CoCApiClientFactory
{
    private static readonly HttpClient _client = new();

    public static string DevelopersKey { get; } = File.ReadAllText(@"./../../../../CustomSolutionElements/ClashApiKey.txt");

    public static HttpClient GetHttpClient()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DevelopersKey);

        return _client;
    }
}