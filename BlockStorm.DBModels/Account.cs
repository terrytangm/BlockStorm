using System;
using System.Collections.Generic;

namespace BlockStorm.EFModels;

public partial class Account
{
    public long Id { get; set; }

    public string PrivateKey { get; set; } = null!;

    public string? Address { get; set; }

    public string? Mnemonic { get; set; }

    public string? Path { get; set; }

    public string? Password { get; set; }

    public string Type { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime Created { get; set; }

    public virtual ICollection<AccountBalance> AccountBalances { get; set; } = new List<AccountBalance>();

    public virtual ICollection<CampaignAccount> CampaignAccounts { get; set; } = new List<CampaignAccount>();

    public virtual ICollection<Campaign> CampaignDeployerAccountNavigations { get; set; } = new List<Campaign>();

    public virtual ICollection<Campaign> CampaignOperatorAccountNavigations { get; set; } = new List<Campaign>();

    public virtual ICollection<Campaign> CampaignWithdrawerAccountNavigations { get; set; } = new List<Campaign>();
}
