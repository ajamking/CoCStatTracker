using System;
using System.Collections.Generic;

namespace CoCApiDealer.UIEntities;

public class CwCwlMembershipUi
{
    public string Tag { get; set; }
    public string Name { get; set; }
    public string ClanTag { get; set; }
    public string ClanName { get; set; }
    public string StartedOn { get; set; }
    public string EndedOn { get; set; }
    public string TownHallLevel { get; set; }
    public string MapPosition { get; set; }
    public string BestOpponentStars { get; set; }
    public string BestOpponentsTime { get; set; }
    public string BestOpponentsPercent { get; set; }
    public ICollection<WarAttackUi> Attacks { get; set; }
}

public class WarAttackUi
{
    public string EnemyTag { get; set; }
    public string EnemyName { get; set; }
    public string AttackOrder { get; set; }
    public string Stars { get; set; }
    public string DestructionPercent { get; set; }
    public string Duration { get; set; }
    public string EnemyTHLevel { get; set; }
    public string EnemyMapPosition { get; set; }
}
