using System.Net.Http.Headers;

namespace CoCApiDealer.RequestsSettings;

public class CoCApiClientFactory
{
    private HttpClient _client = new HttpClient();

    public static string DevelopersKey { get; set; } = File.ReadAllText(@"./DevelopersKey.json");

    public HttpClient GetHttpClient()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DevelopersKey);

        return _client;
    }
}
