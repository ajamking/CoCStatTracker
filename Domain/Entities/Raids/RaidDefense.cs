﻿using System.Collections.Generic;

namespace Domain.Entities;

public class RaidDefense
{
    public int Id { get; set; }
    public string AttackerClanTag { get; set; }
    public string AttackerClanName { get; set; }
    public int AttackerClanLevel { get; set; }
    public int TotalAttacksCount { get; set; }
    public int DistrictsDestroyed { get; set; }

    public int? CapitalRaidId { get; set; }
    public CapitalRaid Raid { get; set; }
    public ICollection<DestroyedFriendlyDistrict> DestroyedFriendlyDistricts { get; set; }

    public RaidDefense()
    {
        DestroyedFriendlyDistricts = new HashSet<DestroyedFriendlyDistrict>();
    }
}
