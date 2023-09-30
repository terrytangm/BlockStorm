using System;
using System.Collections.Generic;

namespace BlockStorm.EFModels;

public partial class CampaignAccount
{
    public long Id { get; set; }

    public long CampaignId { get; set; }

    public long AccountId { get; set; }

    public short? TradeTimes { get; set; }

    public short? BoughtTimes { get; set; }

    public short? SoldTimes { get; set; }

    public string? TradeVolumn { get; set; }

    public string? BoughtVolumn { get; set; }

    public string? SoldVolumn { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? LastUpdate { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Campaign Campaign { get; set; } = null!;
}
