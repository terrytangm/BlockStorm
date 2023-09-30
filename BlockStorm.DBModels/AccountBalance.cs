using System;
using System.Collections.Generic;
using System.Numerics;

namespace BlockStorm.EFModels;

public partial class AccountBalance
{
    public long Id { get; set; }

    public long AccountId { get; set; }

    public long ChainId { get; set; }

    public string? TokenAddress { get; set; }

    public string? TokenName { get; set; }

    public BigInteger Balance { get; set; } 

    public DateTime LastUpdate { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Chain Chain { get; set; } = null!;
}
