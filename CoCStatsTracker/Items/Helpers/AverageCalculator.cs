using Domain.Entities;
using System;

namespace CoCStatsTracker.Items.Helpers;

public static class AverageCalculator
{
    public static int CalculateAveragePercent(ClanMember member, AvgType avgType)
    {
        try
        {
            switch (avgType)
            {
                case AvgType.ClanWar:
                    {
                        if (member.WarMemberships.Count == 0)
                        {
                            return 0;
                        }

                        var warAvg = 0;
                        var warCounter = 0;

                        foreach (var war in member.WarMemberships)
                        {
                            foreach (var attack in war.WarAttacks)
                            {
                                warAvg += attack.DestructionPercent;

                                warCounter++;
                            }
                        }

                        return (warAvg / warCounter);
                    }
                case AvgType.ClanWarWithout1415Th:
                    {
                        if (member.WarMemberships.Count == 0)
                        {
                            return 0;
                        }

                        var warAvg = 0;
                        var warCounter = 0;

                        foreach (var war in member.WarMemberships)
                        {
                            foreach (var attack in war.WarAttacks)
                            {
                                if (attack.EnemyWarMember.TownHallLevel != 15 && attack.EnemyWarMember.TownHallLevel != 14)
                                {
                                    warAvg += attack.DestructionPercent;

                                    warCounter++;
                                }
                            }
                        }

                        return (warAvg / warCounter);
                    }
                case AvgType.Raids:
                    {
                        if (member.RaidMemberships.Count == 0)
                        {
                            return 0;
                        }

                        var raidsAvg = 0;
                        var raidsCounter = 0;

                        foreach (var raid in member.RaidMemberships)
                        {
                            foreach (var attack in raid.Attacks)
                            {
                                raidsAvg += (attack.DestructionPercentTo - attack.DestructionPercentFrom);
                                raidsCounter++;
                            }
                        }

                        return (raidsAvg / raidsCounter);
                    }
                case AvgType.RaidsWithoutPeak:
                    {
                        if (member.RaidMemberships.Count == 0)
                        {
                            return 0;
                        }

                        var raidsAvg = 0;
                        var raidsCounter = 0;

                        foreach (var raid in member.RaidMemberships)
                        {
                            foreach (var attack in raid.Attacks)
                            {
                                if (attack.OpponentDistrict.Name != "Capital Peak")
                                {
                                    raidsAvg += (attack.DestructionPercentTo - attack.DestructionPercentFrom);
                                    raidsCounter++;
                                }
                            }
                        }

                        return (raidsAvg / raidsCounter);
                    }
                default:
                    return 0;
            }
        }
        catch (Exception)
        {
            return (0);
        }
    }
}