using System;
using System.Collections.Generic;

namespace BlockStorm.EFModels;

public partial class TokensAppearTimesInPair
{
    public long ChainId { get; set; }

    public string Token { get; set; } = null!;

    public int? AppearTimes { get; set; }
}
