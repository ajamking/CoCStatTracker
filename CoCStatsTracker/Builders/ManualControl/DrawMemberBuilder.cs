using Domain.Entities;
using System.Collections.Generic;

namespace CoCStatsTracker.Builders;

public class DrawMemberBuilder
{
    public DrawMember Member { get; set; } = new DrawMember();

    public DrawMemberBuilder(DrawMember member = null)
    {
        if (member != null)
        {
            Member = member;
        }
    }

    public void SetBaseProperties()
    {
        Member.TotalPointsEarned = 0;
    }

    public void SetTotalPointsEarned(int currentDrawScore, int carmaindocator)
    {
        Member.TotalPointsEarned += (currentDrawScore + carmaindocator);
    }
}
