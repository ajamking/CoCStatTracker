using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;

public class WarAttackApi
{
    [JsonProperty("attackerTag")]
    public string AttackerTag { get; set; }

    [JsonProperty("defenderTag")]
    public string DefenderTag { get; set; }

    [JsonProperty("stars")]
    public int Stars { get; set; }

    [JsonProperty("destructionPercentage")]
    public int DestructionPercent { get; set; }

    [JsonProperty("order")]
    public int Order { get; set; }

    [JsonProperty("duration")]
    public int Duration { get; set; }
}