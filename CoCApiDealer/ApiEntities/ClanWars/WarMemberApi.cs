using Newtonsoft.Json;

namespace CoCStatsTracker.ApiEntities;
public class WarMemberApi
{
    [JsonProperty("tag")]
    public string Tag { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("townhallLevel")]
    public int TownhallLevel { get; set; }

    [JsonProperty("mapPosition")]
    public int MapPosition { get; set; }

    [JsonProperty("attacks")]
    public WarAttackApi[] Attacks { get; set; }

    [JsonProperty("opponentAttacks")]
    public int OpponentAttacksCount { get; set; }

    [JsonProperty("bestOpponentAttack")]
    public WarAttackApi BestOpponentAttack { get; set; }
}
