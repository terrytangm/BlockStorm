using System;
using System.Collections.Generic;

namespace BlockStorm.EFModels;

public partial class Route
{
    public long Id { get; set; }

    public long ChainId { get; set; }

    public short Hop { get; set; }

    public string TokenIn { get; set; } = null!;

    public string TokenOut { get; set; } = null!;

    public bool Enabled { get; set; }

    public string RouteHash { get; set; } = null!;

    public DateTime Created { get; set; }

    public string? OptimalInput { get; set; }

    public string? OptimalProfit { get; set; }

    public DateTime LastUpdate { get; set; }

    public virtual Chain Chain { get; set; } = null!;

    public virtual ICollection<RouteNode> RouteNodes { get; set; } = new List<RouteNode>();
}
