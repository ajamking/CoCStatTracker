using System.Net.Http.Headers;

namespace CoCApiDealer.RequestsSettings;

public class CoCApiClientFactory
{
    private HttpClient _client = new HttpClient();

    public static string DevelopersKey { get; set; } = File.ReadAllText(@"./../../../../CustomSolutionElements/DevelopersKey.txt");

    public HttpClient GetHttpClient()
    {
        var abc = Environment.CurrentDirectory;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DevelopersKey);

        return _client;
    }
}