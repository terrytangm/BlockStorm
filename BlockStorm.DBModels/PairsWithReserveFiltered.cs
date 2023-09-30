using System;
using System.Collections.Generic;

namespace BlockStorm.EFModels;

public partial class PairsWithReserveFiltered
{
    public string PairAddress { get; set; } = null!;

    public long ChainId { get; set; }

    public string DexName { get; set; } = null!;

    public int? PairIndex { get; set; }

    public string Token0 { get; set; } = null!;

    public string Token1 { get; set; } = null!;

    public string Reserve0 { get; set; } = null!;

    public string Reserve1 { get; set; } = null!;

    public int BlockTimeLast { get; set; }

    public short? Fee { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime? Created { get; set; }

    public bool? Token0In { get; set; }

    public bool? Token1In { get; set; }
}
