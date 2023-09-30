using System;
using System.Collections.Generic;

namespace BlockStorm.EFModels;

public partial class SyncReserveLog
{
    public long Id { get; set; }

    public string? PairAddress { get; set; }

    public int? ChainId { get; set; }

    public string? DexName { get; set; }

    public string? Reserve0 { get; set; }

    public string? Reserve1 { get; set; }

    public long? BlockNumber { get; set; }

    public string? BlockHash { get; set; }

    public string? TransactionHash { get; set; }

    public DateTime? Created { get; set; }
}
