using System;
using System.Collections.Generic;

namespace BlockStorm.EFModels;

public partial class Token
{
    public string TokenAddress { get; set; } = null!;

    public long ChainId { get; set; }

    public byte? Decimals { get; set; }

    public string? Name { get; set; }

    public string? Symbol { get; set; }

    public string? TotalSupply { get; set; }

    public DateTime Created { get; set; }

    public string? NativeTokenBalance { get; set; }

    public bool? IsTopToken { get; set; }

    public DateTime? LastBalanceUpdate { get; set; }

    public string? LowestReserve { get; set; }

    public decimal? PriceUsdt { get; set; }

    public DateTime? LastUpdate { get; set; }

    public string? PriceSymbol { get; set; }

    public long? DeployerID { get; set; }

    public long? CampaignID { get; set; }

    public string? FuncSig { get; set; }

    public virtual Chain Chain { get; set; } = null!;
}
