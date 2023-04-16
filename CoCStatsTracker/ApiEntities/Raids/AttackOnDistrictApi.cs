using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;
public class AttackOnDistrictApi
{
    [JsonProperty("attacker")]
    public AttackerShortInfoApi Attacker { get; set; }

    [JsonProperty("destructionPercent")]
    public int DestructionPercentTo { get; set; }

    [JsonProperty("stars")]
    public int StarsTo { get; set; } // В бд не идет
}

public class AttackerShortInfoApi
{
    [JsonProperty("tag")]
    public string Tag { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}