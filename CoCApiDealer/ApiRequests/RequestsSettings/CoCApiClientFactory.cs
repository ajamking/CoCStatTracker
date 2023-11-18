using System.Net.Http.Headers;

namespace CoCApiDealer.ApiRequests;

public static class CoCApiClientFactory
{
    private static readonly HttpClient _client = new();

    private static string _developersKeyPC { get; } = File.ReadAllText(@"./../../../../CustomSolutionElements/ClashApiKeyPC.txt");

    private static string _developersKeyVM { get; } = File.ReadAllText(@"./../../../../CustomSolutionElements/ClashApiKeyVM.txt");

    public static string DevelopersKey { get; } = _developersKeyPC;

    public static HttpClient GetHttpClient()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DevelopersKey);

        return _client;
    }
}