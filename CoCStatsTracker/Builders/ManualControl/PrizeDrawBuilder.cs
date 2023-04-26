using Domain.Entities;
using System;
using System.Collections.Generic;

namespace CoCStatsTracker.Builders.ManualControl;

public class PrizeDrawBuilder
{
    public PrizeDraw Draw { get; } = new PrizeDraw();

    public PrizeDrawBuilder(PrizeDraw draw = null)
    {
        if (draw != null)
        {
            Draw = draw;
        }
    }

    public void SetBaseProperties(DateTime start, DateTime end, string desctiption)
    {
        Draw.StartedOn = start;
        Draw.EndedOn = end;
        Draw.Description = desctiption;
        Draw.WinnerName = "Not determined yet";
        Draw.WinnerTotalScore = 0;
    }

    public void SetTrackedClan(TrackedClan clan)
    {
        Draw.TrackedClan = clan;
    }

    public void SetDrawMembers(ICollection<DrawMember> members)
    {
        Draw.Members = members;
    }
}
