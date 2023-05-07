using Domain.Entities;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCApiDealer;
/*
1.  Боевые звезды – 1 звезда – 0,5 балла.
2.  Донат – каждые 100 мест – 1 балл, максимум 40 баллов.
3.  Пожертвования в столице – 1000 золота столицы – 0,5 балла.
 */

public static class DrawDealer
{
    public static Dictionary<string, double> Coefficients { get; set; } = new Dictionary<string, double>()
    {
        {"WarStars", 0.5 },
        {"DonationsSent", 0.01 },
        {"TotalCapitalContributions", 0.0005 },
    };

    public static List<DrawMember> RecalculatePrizeDrawScores(TrackedClan previousClanInfo, TrackedClan currentClanInfo,
        ICollection<DrawMember> drawMembers)
    {

        var drawMembersWithUpdatedScores = new List<DrawMember>();

        var tempClanMembers = drawMembers
            .Select(x => x.ClanMember)
            .IntersectBy(previousClanInfo.ClanMembers.Select(x => x.Tag), x => x.Tag)
            .IntersectBy(currentClanInfo.ClanMembers.Select(x => x.Tag), x => x.Tag)
            .ToList();

        foreach (var clanMember in tempClanMembers)
        {
            var newWarStars =
                (currentClanInfo.ClanMembers.First(x => x.Tag == clanMember.Tag).WarStars -
                previousClanInfo.ClanMembers.First(x => x.Tag == clanMember.Tag).WarStars) * Coefficients["WarStars"];

            var newCapitalContributions =
                (currentClanInfo.ClanMembers.First(x => x.Tag == clanMember.Tag).TotalCapitalContributions -
                previousClanInfo.ClanMembers.First(x => x.Tag == clanMember.Tag).TotalCapitalContributions) * Coefficients["TotalCapitalContributions"];

            var newDonationsSent = currentClanInfo.ClanMembers.First(x => x.Tag == clanMember.Tag).DonationsSent * Coefficients["DonationsSent"];

            var newCarma = clanMember.Carma.TotalCarma;

            var updatedDrawMember = drawMembers.First(x => x.ClanMember.Tag == clanMember.Tag);

            updatedDrawMember.TotalPointsEarned = (int)(newWarStars + newCapitalContributions + newDonationsSent + newCarma);

            drawMembersWithUpdatedScores.Add(updatedDrawMember);
        }

        return drawMembersWithUpdatedScores;
    }

    public static PrizeDraw ChoseDrawWinner(PrizeDraw draw)
    {
        var bestScoreMember = new DrawMember() { TotalPointsEarned = 0 };

        foreach (var member in draw.Members)
        {
            if (member.TotalPointsEarned > bestScoreMember.TotalPointsEarned)
            {
                bestScoreMember = member;
            }
        }

        draw.WinnerName = @$"{bestScoreMember.ClanMember.Name} [{bestScoreMember.ClanMember.Tag}]";
        draw.WinnerTotalScore = bestScoreMember.TotalPointsEarned;

        return draw;
    }
}
