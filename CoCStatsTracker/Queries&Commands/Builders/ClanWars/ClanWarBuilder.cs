using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Items.Helpers;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace CoCStatsTracker.Builders;

public class ClanWarBuilder
{
    public ClanWar ClanWar { get; }

    public ClanWarBuilder(ClanWar clanWar = null)
    {
        ClanWar = clanWar ?? new ClanWar();
    }

    public void SetBaseProperties(ClanWarApi clanWarApi, bool isCwlWar = false, int attacksPerMember = 2)
    {
        ClanWar.UpdatedOn = DateTime.Now;

        ClanWar.IsCWL = isCwlWar;
        ClanWar.Result = GetWarResult(clanWarApi);

        ClanWar.State = clanWarApi.State;
        ClanWar.TeamSize = clanWarApi.TeamSize;
        ClanWar.AttackPerMember = attacksPerMember;

        ClanWar.PreparationStartTime = DateTimeParser.ParseToDateTime(clanWarApi.PreparationStartTime).ToLocalTime();
        ClanWar.StartedOn = DateTimeParser.ParseToDateTime(clanWarApi.StartTime).ToLocalTime();
        ClanWar.EndedOn = DateTimeParser.ParseToDateTime(clanWarApi.EndTime).ToLocalTime();

        ClanWar.AttacksCount = clanWarApi.ClanResults.AttacksCount;
        ClanWar.StarsCount = clanWarApi.ClanResults.StarsCount;
        ClanWar.DestructionPercentage = clanWarApi.ClanResults.DestructionPercentage;

        ClanWar.OpponentClanTag = clanWarApi.OpponentResults.Tag;
        ClanWar.OpponentClanName = clanWarApi.OpponentResults.Name;
        ClanWar.OpponentClanLevel = clanWarApi.OpponentResults.ClanLevel;
        ClanWar.OpponentAttacksCount = clanWarApi.OpponentResults.AttacksCount;
        ClanWar.OpponentStarsCount = clanWarApi.OpponentResults.StarsCount;
        ClanWar.OpponentDestructionPercentage = clanWarApi.OpponentResults.DestructionPercentage;
    }

    public void SetTrackedClan(TrackedClan clan)
    {
        ClanWar.TrackedClan = clan;
    }

    public void SetWarMembers(ICollection<WarMember> warMembers)
    {
        ClanWar.WarMembers = warMembers;
    }

    public void SetEnemyWarMembers(ICollection<EnemyWarMember> warMembers)
    {
        ClanWar.EnemyWarMembers = warMembers;
    }

    private static string GetWarResult(ClanWarApi clanWarApi)
    {
        var result = "Неопределен";

        if (clanWarApi.State == "warEnded")
        {
            if (clanWarApi.ClanResults.StarsCount > clanWarApi.OpponentResults.StarsCount)
            {
                result = "Победа";
            }
            else if (clanWarApi.ClanResults.StarsCount == clanWarApi.OpponentResults.StarsCount)
            {
                if (clanWarApi.ClanResults.DestructionPercentage > clanWarApi.OpponentResults.DestructionPercentage)
                {
                    result = "Победа";
                }
                else
                {
                    result = "Поражение";
                }
            }
            else
            {
                result = "Поражение";
            }
        }

        return result;
    }
}