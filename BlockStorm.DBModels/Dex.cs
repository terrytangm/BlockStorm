using System;
using System.Collections.Generic;

namespace BlockStorm.EFModels;

public partial class Dex
{
    public string DexName { get; set; } = null!;

    public long ChainId { get; set; }

    public string Factory { get; set; } = null!;

    public string Router { get; set; } = null!;

    public short? Fee { get; set; }

    public DateTime? Created { get; set; }

    public virtual Chain Chain { get; set; } = null!;

    public virtual ICollection<Pair> Pairs { get; set; } = new List<Pair>();
}
