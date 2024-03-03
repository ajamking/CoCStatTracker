using CoCStatsTracker.UIEntities.ClanInfo;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CoCStatsTracker.Items.Helpers;

public static class NonAttackersHelper
{
    public static List<NonAttacker> GetNonAttackersRaids(TrackedClan clan)
    {
        var membersWithoutAttacks = new List<NonAttacker>();

        var count = 0;

        var raid = clan.CapitalRaids.OrderByDescending(x => x.StartedOn).FirstOrDefault();

        var isAnyApsent = false;

        foreach (var member in clan.ClanMembers)
        {
            if (member.RaidMemberships.FirstOrDefault(x => x.CapitalRaid.StartedOn == raid.StartedOn) == null)
            {
                isAnyApsent = true;
            }
        }

        if (isAnyApsent)
        {
            foreach (var clanMember in clan.ClanMembers)
            {
                if (raid.RaidMembers.FirstOrDefault(x => x.Tag == clanMember.Tag) == null)
                {
                    membersWithoutAttacks.Add(new NonAttacker()
                    {
                        Tag = clanMember.Name,
                        Name = clanMember.Name,
                        AttacksCount = 0,
                        TelegramUserName = clanMember.TelegramUserName
                    });

                    count++;
                }
            }
        }

        if (raid.RaidMembers.Any(x => x.Attacks.Count != 6))
        {
            foreach (var raidMember in raid.RaidMembers)
            {
                if (raidMember.Attacks.Count != 6)
                {
                    membersWithoutAttacks.Add(new NonAttacker()
                    {
                        Tag = raidMember.Tag,
                        Name = raidMember.Name,
                        AttacksCount = raidMember.Attacks.Count,
                        TelegramUserName = TryGetTelegramUserName(raidMember.Tag)
                    });

                    count++;
                }
            }
        }

        return membersWithoutAttacks;
    }

    public static List<NonAttacker> GetNonAttackersCw(ClanWar clanWar)
    {
        var membersWithoutAttacks = new List<NonAttacker>();

        if (clanWar.WarMembers.Any(x => x.WarAttacks.Count != 1))
        {
            foreach (var warMember in clanWar.WarMembers)
            {
                if (warMember.WarAttacks.Count < clanWar.AttackPerMember)
                {
                    membersWithoutAttacks.Add(new NonAttacker()
                    {
                        Name = warMember.Name,
                        Tag = warMember.Tag,
                        AttacksCount = warMember.WarAttacks.Count,
                        TelegramUserName = TryGetTelegramUserName(warMember.Tag)
                    });
                }
            }
        }

        return membersWithoutAttacks;
    }

    private static string TryGetTelegramUserName(string memberTag)
    {
        try
        {
            var clanMemberUi = GetFromDbQueryHandler.GetClanMemberUi(memberTag);

            var result = "";

            if (clanMemberUi != null && clanMemberUi.TelegramUserName != null)
            {
                result = clanMemberUi.TelegramUserName;
            }

            return result;
        }
        catch
        {
            return "";
        }
    }
}