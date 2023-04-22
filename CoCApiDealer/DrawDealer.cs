using Domain.Entities;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCApiDealer;
/*
1.  Боевые звезды – 1 звезда – 0,5 балла.
2.  Донат – каждые 50 мест – 1 балл, максимум 30 баллов.
3.  Пожертвования в столице – 1000 золота столицы – 0,5 балла.

___________________Фиксируем через PlayerActivities_________________
4.  Очки на ИК – каждые 500 очков – 1 балл. - фиксируем через PlayerActivities
5.  Нововведения, одобренные в клане – на усмотрение главы/соруководителей – максимум 20 баллов.
6.  Активность в пятничных тренировках – на усмотрение главы/соруководителей/старейшин – максимум 10 баллов.
7.  Прочая активность – на усмотрение главы/соруководителей – максимум 40 баллов. 
“Реферальная программа также учитывается в этом показателе – 1 
человек с 11+ ТХ – 10 баллов, максимум 30 баллов в месяц.
 */



public class DrawDealer
{
    public Dictionary<string, double> Coefficients { get; } = new Dictionary<string, double>()
    {
        {"WarStars", 0.5 },
        {"DonationsSent", 0.02 },
        {"TotalCapitalContributions", 0.0005 },
    };

    public void RecalculatePrizeDrawScores(TrackedClan previousClanInfo, TrackedClan currentClanInfo, PrizeDraw Draw)
    {

        foreach (var member in Draw.Members)
        {

        }

    }

    public DrawMember ChoseDrawWinner(PrizeDraw Draw)
    {
        var bestScoreMember = new DrawMember() { TotalPointsEarned = 0 };

        foreach (var member in Draw.Members)
        {
            if (member.TotalPointsEarned>bestScoreMember.TotalPointsEarned)
            {
                bestScoreMember = member;
            }
        }

        return bestScoreMember;
    }
}
