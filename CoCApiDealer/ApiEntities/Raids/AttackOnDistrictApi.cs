using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;

public class AttackOnDistrictApi
{
    [JsonProperty("attacker")]
    public AttackerShortInfoApi Attacker { get; set; }

    [JsonProperty("destructionPercent")]
    public int DestructionPercentTo { get; set; }

    // public int DestructionPercentForm { get =>  } //Отказались пока от этой идеи, сложно реализовать

    [JsonProperty("stars")]
    public int StarsTo { get; set; } // Не используем
}

public class AttackerShortInfoApi
{
    [JsonProperty("tag")]
    public string Tag { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}