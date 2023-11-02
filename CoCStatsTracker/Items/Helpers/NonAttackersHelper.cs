using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CoCStatsTracker.Items.Helpers;

public static class NonAttackersHelper
{
    public static Dictionary<string, int> GetNonAttackersRaids(TrackedClan clan)
    {
        var membersWithout6Attacks = new Dictionary<string, int>();

        var count = 0;

        var raid = clan.CapitalRaids.OrderByDescending(x => x.StartedOn).FirstOrDefault();

        var isAnyApsent = false;

        foreach (var member in clan.ClanMembers)
        {
            if (member.RaidMemberships.FirstOrDefault(x => x.Raid.StartedOn == raid.StartedOn) == null)
            {
                isAnyApsent = true;
            }
        }

        if (isAnyApsent)
        {
            foreach (var clanMember in clan.ClanMembers)
            {
                if (raid.RaidMembers.FirstOrDefault(x => x.MemberTag == clanMember.Tag) == null)
                {
                    membersWithout6Attacks.Add(clanMember.Name, 0);

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
                    membersWithout6Attacks.Add(raidMember.MemberName, raidMember.Attacks.Count);

                    count++;
                }
            }
        }

        if (count is not 0)
        {
            return membersWithout6Attacks;
        }
        else
        {
            return null;
        }
    }

    public static Dictionary<string, int> GetNonAttackersCw(ClanWar clanWar)
    {
        var membersWithoutAttacks = new Dictionary<string, int>();

        if (clanWar.WarMembers.Any(x => x.WarAttacks.Count != 1))
        {
            foreach (var warMember in clanWar.WarMembers)
            {
                if (warMember.WarAttacks.Count < clanWar.AttackPerMember)
                {
                    membersWithoutAttacks.Add(warMember.Name, warMember.WarAttacks.Count);
                }
            }
        }

        return membersWithoutAttacks;
    }
}

