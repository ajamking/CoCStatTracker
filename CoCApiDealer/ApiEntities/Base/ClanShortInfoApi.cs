using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;

/// <summary>
/// Пока что нахуй не нужный класс
/// </summary>
public class ClanShortInfoApi
{
    [JsonProperty("tag")]
    public string Tag { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("clanLevel")]
    public int ClanLevel { get; set; }
}
