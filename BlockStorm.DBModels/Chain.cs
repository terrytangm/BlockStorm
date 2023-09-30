using System;
using System.Collections.Generic;

namespace BlockStorm.EFModels;

public partial class Chain
{
    public long ChainId { get; set; }

    public string? ChainName { get; set; }

    public string? Currency { get; set; }

    public short? Decimals { get; set; }

    public virtual ICollection<AccountBalance> AccountBalances { get; set; } = new List<AccountBalance>();

    public virtual ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();

    public virtual ICollection<Dex> Dices { get; set; } = new List<Dex>();

    public virtual ICollection<Pair> Pairs { get; set; } = new List<Pair>();

    public virtual ICollection<Route> Routes { get; set; } = new List<Route>();

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
