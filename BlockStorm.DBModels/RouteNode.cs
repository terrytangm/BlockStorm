using System;
using System.Collections.Generic;

namespace BlockStorm.EFModels;

public partial class RouteNode
{
    public long Id { get; set; }

    public long RouteId { get; set; }

    public short PairRank { get; set; }

    public string Pair { get; set; } = null!;

    public string TokenIn { get; set; } = null!;

    public string TokenOut { get; set; } = null!;

    public DateTime Created { get; set; }

    public virtual Route Route { get; set; } = null!;
}
