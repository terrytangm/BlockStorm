using System;
using System.Collections.Generic;

namespace BlockStorm.EFModels;

public partial class Campaign
{
    public long Id { get; set; 
    }

    public long ChainId { get; set; }

    public string? TokenAddress { get; set; }

    public string? TokenName { get; set; }

    public string? FuncSig { get; set; }

    public long? DeployerAccount { get; set; }

    public long? OperatorAccount { get; set; }

    public long? WithdrawerAccount { get; set; }

    public string? InitialBalance { get; set; }

    public string? FinalBalance { get; set; }

    public string? ClosedAmount { get; set; }

    public string? NetProfit { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? LastUpdate { get; set; }

    public long? LpBlock { get; set; }

    public long? LastProcessedBlock { get; set; }

    public virtual ICollection<CampaignAccount> CampaignAccounts { get; set; } = new List<CampaignAccount>();

    public virtual Chain Chain { get; set; } = null!;

    public virtual Account? DeployerAccountNavigation { get; set; }

    public virtual Account? OperatorAccountNavigation { get; set; }

    public virtual Account? WithdrawerAccountNavigation { get; set; }
}
