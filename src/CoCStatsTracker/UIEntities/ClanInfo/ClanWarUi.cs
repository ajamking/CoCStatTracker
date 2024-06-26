﻿using CoCStatsTracker.UIEntities.ClanInfo;
using System;
using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;

public class ClanWarUi : UiEntity
{
    public bool IsCwl { get; set; }
    public DateTime PreparationStartTime { get; set; }
    public DateTime StartedOn { get; set; }
    public DateTime EndedOn { get; set; }
    public int WarMembersCount { get; set; }
    public int AttackPerMember { get; set; }
    public int AttacksCount { get; set; }

    public string ClanTag { get; set; }
    public string ClanName { get; set; }
    public int TotalStarsEarned { get; set; }
    public double DestructionPercentage { get; set; }

    public string OpponentName { get; set; }
    public string OpponentTag { get; set; }
    public int OpponentStarsCount { get; set; }
    public double OpponentDestructionPercentage { get; set; }

    public int OpponentAttacksCount { get; set; }
    public string Result { get; set; }

    public int OpponentWarWinStreak { get; set; }
    public int OppinentWarWins { get; set; }
    public int OppinentWarLoses { get; set; }
    public int OppinentWarDraws { get; set; }

    public WarMapUi WarMap { get; set; }
    public ICollection<NonAttacker> NonAttackersCw { get; set; }
    public ICollection<ClanWarAttackUi> MembersResults { get; set; } // Для графика по всему клану
}

public class ClanWarAttackUi : UiEntity
{
    public string PlayerName { get; set; }
    public int ThLevel { get; set; }

    public int FirstEnemyThLevel { get; set; }
    public int FirstStarsCount { get; set; }
    public int FirstDestructionPercent { get; set; }

    public int SecondEnemyThLevel { get; set; }
    public int SecondStarsCount { get; set; }
    public int SecondDestructionpercent { get; set; }
}