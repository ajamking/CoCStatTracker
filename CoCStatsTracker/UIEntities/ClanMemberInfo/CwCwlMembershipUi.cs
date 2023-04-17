using System;
using System.Collections.Generic;

namespace CoCApiDealer.UIEntities;

public class CwCwlMembershipUi
{
    public string Tag { get; set; }
    public string Name { get; set; }
    public string ClanTag { get; set; }
    public string ClanName { get; set; }
    public DateTime StartedOn { get; set; }
    public DateTime EndedOn { get; set; }
    public int TownHallLevel { get; set; }
    public int MapPosition { get; set; }
    public int BestOpponentStars { get; set; }
    public int BestOpponentsTime { get; set; }
    public int BestOpponentsPercent { get; set; }
    public ICollection<WarAttackUi> Attacks { get; set; }
}

public class WarAttackUi
{
    public string EnemyTag { get; set; }
    public string EnemyName { get; set; }
    public int AttackOrder { get; set; }
    public int Stars { get; set; }
    public int DestructionPercent { get; set; }
    public int Duration { get; set; }
    public int EnemyTHLevel { get; set; }
    public int EnemyMapPosition { get; set; }
}
