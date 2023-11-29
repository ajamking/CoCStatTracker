using System.Net.Http.Headers;

namespace CoCApiDealer.ApiRequests;

public static class CoCApiClientFactory
{
    private static readonly HttpClient _client = new();

    private static readonly string _developersKeyPC = File.ReadAllText(@"./../../../../CustomSolutionElements/ClashApiKeyPC.txt");

    private static readonly string _developersKeyVM = File.ReadAllText(@"./../../../../CustomSolutionElements/ClashApiKeyVM.txt");

    public static string DevelopersKey { get; } = _developersKeyVM;

    public static HttpClient GetHttpClient()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DevelopersKey);

        return _client;
    }
}