using Domain.Entities;
using System;

namespace CoCStatsTracker.Builders;

public class CarmaBuilder
{
    public Carma PlayersCarma { get; } = new Carma();

    public CarmaBuilder(Carma carma = null)
    {
        if (carma != null)
        {
            PlayersCarma = carma;
            PlayersCarma.UpdatedOn = DateTime.Now.ToLocalTime();
        }
    }

    public void SetActivity(string name, string description, int points)
    {
        PlayersCarma.PlayerActivities.Add(new CustomActivity
        {
            Name = name,
            Description = description,
            EarnedPoints = points,
            UpdatedOn = DateTime.Now.ToLocalTime(),
        });

        PlayersCarma.TotalCarma += points;
        PlayersCarma.UpdatedOn = DateTime.Now.ToLocalTime();
    }
}
