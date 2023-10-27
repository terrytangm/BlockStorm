using System;
using System.Collections.Generic;

namespace BlockStorm.EFModels;

public partial class ClosingDoorRecord
{
    public long Id { get; set; }

    public long CamapaignId { get; set; }

    public string TraderAddress { get; set; } = null!;

    public decimal? Ethamount { get; set; }

    public decimal? TokenAmount { get; set; }

    public long? BlockNumber { get; set; }

    public string? TransactionHash { get; set; }

    public bool Closed { get; set; }
}
