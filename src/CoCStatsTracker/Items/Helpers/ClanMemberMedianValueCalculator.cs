using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoCStatsTracker.Items.Helpers;

public static class ClanMemberMedianValueCalculator
{
    public static int Calculate(ClanMember member, MedianValueType avgType)
    {
        try
        {
            switch (avgType)
            {
                case MedianValueType.ClanWar:
                    {
                        if (member.WarMemberships.Count == 0 || !member.WarMemberships.Select(x => x.WarAttacks).Any())
                        {
                            return 0;
                        }

                        var warAttacks = new List<WarAttack>();

                        foreach (var warMember in member.WarMemberships)
                        {
                            warAttacks.AddRange(warMember.WarAttacks);
                        }

                        var sortedAttacks = warAttacks.OrderByDescending(x => x.DestructionPercent).ToList();

                        if (sortedAttacks.Count == 0) { return 0; }

                        return sortedAttacks[sortedAttacks.Count / 2].DestructionPercent;
                    }
                case MedianValueType.ClanWarWithout1415Th:
                    {
                        if (member.WarMemberships.Count == 0 || !member.WarMemberships.Select(x => x.WarAttacks).Any())
                        {
                            return 0;
                        }

                        var warAttacks = new List<WarAttack>();

                        foreach (var warMember in member.WarMemberships)
                        {
                            warAttacks.AddRange(warMember.WarAttacks.Where(x => x.EnemyWarMember.TownHallLevel is not 15 or 14));
                        }

                        var sortedAttacks = warAttacks.OrderByDescending(x => x.DestructionPercent).ToList();

                        if (sortedAttacks.Count == 0) { return 0; }

                        return sortedAttacks[sortedAttacks.Count / 2].DestructionPercent;
                    }
                case MedianValueType.Raids:
                    {
                        if (member.RaidMemberships.Count == 0 || !member.RaidMemberships.Select(x => x.Attacks).Any())
                        {
                            return 0;
                        }

                        var raidAttacks = new List<RaidAttack>();

                        foreach (var raidMember in member.RaidMemberships)
                        {
                            raidAttacks.AddRange(raidMember.Attacks);
                        }

                        var sortedAttacks = raidAttacks.OrderByDescending(x => x.DestructionPercentTo - x.DestructionPercentFrom).ToList();

                        if (sortedAttacks.Count == 0) { return 0; }

                        var resultAttack = sortedAttacks[sortedAttacks.Count / 2];

                        return resultAttack.DestructionPercentTo - resultAttack.DestructionPercentFrom;
                    }
                case MedianValueType.RaidsWithoutPeak:
                    {
                        if (member.RaidMemberships.Count == 0 || !member.RaidMemberships.Select(x => x.Attacks).Any())
                        {
                            return 0;
                        }

                        var raidAttacks = new List<RaidAttack>();

                        foreach (var raidMember in member.RaidMemberships)
                        {
                            raidAttacks.AddRange(raidMember.Attacks.Where(x => x.DefeatedEmemyDistrict.Name != "Capital Peak"));
                        }

                        var sortedAttacks = raidAttacks.OrderByDescending(x => x.DestructionPercentTo - x.DestructionPercentFrom).ToList();

                        if (sortedAttacks.Count == 0) { return 0; }

                        var resultAttack = sortedAttacks[sortedAttacks.Count / 2];

                        return resultAttack.DestructionPercentTo - resultAttack.DestructionPercentFrom;
                    }
                default:
                    return 0;
            }
        }
        catch (Exception)
        {
            return 0;
        }
    }
}