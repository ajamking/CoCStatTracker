using CoCApiDealer;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CoCStatsTracker.BusinessFunctions;
 
public class TrackedClanCreator
{
    public TrackedClan Clan { get; }

    public TrackedClanCreator(string tag)
    {
        Clan = new TrackedClan() { Tag = tag };
    }

    private void SetClanBaseProperties()
    {

    }
}
