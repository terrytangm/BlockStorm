using System;
using System.Collections.Generic;
using BlockStorm.Utils;
using Microsoft.EntityFrameworkCore;

namespace BlockStorm.EFModels;

public partial class BlockchainContext : DbContext
{
    public BlockchainContext()
    {
    }

    public BlockchainContext(DbContextOptions<BlockchainContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountBalance> AccountBalances { get; set; }

    public virtual DbSet<Campaign> Campaigns { get; set; }

    public virtual DbSet<CampaignAccount> CampaignAccounts { get; set; }

    public virtual DbSet<Chain> Chains { get; set; }

    public virtual DbSet<Dex> Dexes { get; set; }

    public virtual DbSet<FilteredPair> FilteredPairs { get; set; }

    public virtual DbSet<Pair> Pairs { get; set; }

    public virtual DbSet<PairsWithReserveFiltered> PairsWithReserveFiltereds { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<RouteNode> RouteNodes { get; set; }

    public virtual DbSet<SyncReserveLog> SyncReserveLogs { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<ClosingDoorRecord> ClosingDoorRecords { get; set; }

    public virtual DbSet<TokensAppearTimesInPair> TokensAppearTimesInPairs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Config.GetConnectionString("BlockchainDB"), builder =>
        {
            builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Wallet");

            entity.ToTable("Account");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Mnemonic)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Path)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PrivateKey)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AccountBalance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_WalletBalance");

            entity.ToTable("AccountBalance");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            
            entity.Property(e => e.ChainId).HasColumnName("ChainID");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.TokenAddress)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TokenName)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.AccountBalances)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wallet_WalletBalance");

            entity.HasOne(d => d.Chain).WithMany(p => p.AccountBalances)
                .HasForeignKey(d => d.ChainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chain_1_ChainID");
        });

        modelBuilder.Entity<Campaign>(entity =>
        {
            entity.ToTable("Campaign");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ChainId).HasColumnName("ChainID");
            entity.Property(e => e.ClosedAmount)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.FinalBalance)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FuncSig)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.InitialBalance)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.NetProfit)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.TokenAddress)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TokenName)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.Chain).WithMany(p => p.Campaigns)
                .HasForeignKey(d => d.ChainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Campaign_Chain");

            entity.HasOne(d => d.DeployerAccountNavigation).WithMany(p => p.CampaignDeployerAccountNavigations)
                .HasForeignKey(d => d.DeployerAccount)
                .HasConstraintName("FK_Campaign_Account");

            entity.HasOne(d => d.OperatorAccountNavigation).WithMany(p => p.CampaignOperatorAccountNavigations)
                .HasForeignKey(d => d.OperatorAccount)
                .HasConstraintName("FK_Campaign_Account1");

            entity.HasOne(d => d.WithdrawerAccountNavigation).WithMany(p => p.CampaignWithdrawerAccountNavigations)
                .HasForeignKey(d => d.WithdrawerAccount)
                .HasConstraintName("FK_Campaign_Account2");
        });

        modelBuilder.Entity<CampaignAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CampaignWallet");

            entity.ToTable("CampaignAccount");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.BoughtVolumn)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CampaignId).HasColumnName("CampaignID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.SoldVolumn)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TradeVolumn)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.CampaignAccounts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CampaignWallet_Wallet");

            entity.HasOne(d => d.Campaign).WithMany(p => p.CampaignAccounts)
                .HasForeignKey(d => d.CampaignId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CampaignWallet_Campaign");
        });

        modelBuilder.Entity<Chain>(entity =>
        {
            entity.ToTable("Chain");

            entity.Property(e => e.ChainId)
                .ValueGeneratedNever()
                .HasColumnName("ChainID");
            entity.Property(e => e.ChainName).HasMaxLength(50);
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<ClosingDoorRecord>(entity =>
        {
            entity.ToTable("ClosingDoorRecord");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CamapaignId).HasColumnName("CamapaignID");
            entity.Property(e => e.Ethamount)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("ETHAmount");
            entity.Property(e => e.TokenAmount).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TraderAddress)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionHash)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Dex>(entity =>
        {
            entity.HasKey(e => e.DexName);

            entity.ToTable("Dex");

            entity.Property(e => e.DexName).HasMaxLength(50);
            entity.Property(e => e.ChainId).HasColumnName("ChainID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Factory)
                .HasMaxLength(42)
                .IsFixedLength();
            entity.Property(e => e.Router)
                .HasMaxLength(42)
                .IsFixedLength();

            entity.HasOne(d => d.Chain).WithMany(p => p.Dices)
                .HasForeignKey(d => d.ChainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Dex_Chain");
        });

        modelBuilder.Entity<FilteredPair>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("FilteredPairs");

            entity.Property(e => e.ChainId).HasColumnName("ChainID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.DexName).HasMaxLength(50);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.PairAddress)
                .HasMaxLength(42)
                .IsFixedLength();
            entity.Property(e => e.Reserve0)
                .HasMaxLength(90)
                .IsFixedLength();
            entity.Property(e => e.Reserve1)
                .HasMaxLength(90)
                .IsFixedLength();
            entity.Property(e => e.Token0)
                .HasMaxLength(42)
                .IsFixedLength();
            entity.Property(e => e.Token1)
                .HasMaxLength(42)
                .IsFixedLength();
        });

        modelBuilder.Entity<Pair>(entity =>
        {
            entity.HasKey(e => e.PairAddress);

            entity.ToTable("Pair");

            entity.HasIndex(e => e.Token0, "Token0_Pair");

            entity.HasIndex(e => e.Token1, "Token1_Pair");

            entity.Property(e => e.PairAddress)
                .HasMaxLength(42)
                .IsFixedLength();
            entity.Property(e => e.ChainId).HasColumnName("ChainID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.DexName).HasMaxLength(50);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Reserve0)
                .HasMaxLength(90)
                .IsFixedLength();
            entity.Property(e => e.Reserve1)
                .HasMaxLength(90)
                .IsFixedLength();
            entity.Property(e => e.Token0)
                .HasMaxLength(42)
                .IsFixedLength();
            entity.Property(e => e.Token1)
                .HasMaxLength(42)
                .IsFixedLength();

            entity.HasOne(d => d.Chain).WithMany(p => p.Pairs)
                .HasForeignKey(d => d.ChainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pair_Chain");

            entity.HasOne(d => d.DexNameNavigation).WithMany(p => p.Pairs)
                .HasForeignKey(d => d.DexName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pair_Dex");
        });

        modelBuilder.Entity<PairsWithReserveFiltered>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("PairsWithReserveFiltered");

            entity.Property(e => e.ChainId).HasColumnName("ChainID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.DexName).HasMaxLength(50);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.PairAddress)
                .HasMaxLength(42)
                .IsFixedLength();
            entity.Property(e => e.Reserve0)
                .HasMaxLength(90)
                .IsFixedLength();
            entity.Property(e => e.Reserve1)
                .HasMaxLength(90)
                .IsFixedLength();
            entity.Property(e => e.Token0)
                .HasMaxLength(42)
                .IsFixedLength();
            entity.Property(e => e.Token1)
                .HasMaxLength(42)
                .IsFixedLength();
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.ToTable("Route");

            entity.HasIndex(e => e.RouteHash, "RouteHash_Route");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ChainId).HasColumnName("ChainID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.OptimalInput)
                .HasMaxLength(90)
                .IsFixedLength();
            entity.Property(e => e.OptimalProfit)
                .HasMaxLength(90)
                .IsFixedLength();
            entity.Property(e => e.RouteHash)
                .HasMaxLength(300)
                .IsFixedLength();
            entity.Property(e => e.TokenIn)
                .HasMaxLength(42)
                .IsFixedLength();
            entity.Property(e => e.TokenOut)
                .HasMaxLength(42)
                .IsFixedLength();

            entity.HasOne(d => d.Chain).WithMany(p => p.Routes)
                .HasForeignKey(d => d.ChainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Route_Chain");
        });

        modelBuilder.Entity<RouteNode>(entity =>
        {
            entity.ToTable("RouteNode");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Pair)
                .HasMaxLength(42)
                .IsFixedLength();
            entity.Property(e => e.RouteId).HasColumnName("RouteID");
            entity.Property(e => e.TokenIn)
                .HasMaxLength(42)
                .IsFixedLength();
            entity.Property(e => e.TokenOut)
                .HasMaxLength(42)
                .IsFixedLength();

            entity.HasOne(d => d.Route).WithMany(p => p.RouteNodes)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RouteNode_Route");
        });

        modelBuilder.Entity<SyncReserveLog>(entity =>
        {
            entity.ToTable("SyncReserveLog");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BlockHash)
                .HasMaxLength(70)
                .IsFixedLength();
            entity.Property(e => e.ChainId).HasColumnName("chainID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.DexName).HasMaxLength(200);
            entity.Property(e => e.PairAddress)
                .HasMaxLength(42)
                .IsFixedLength()
                .HasColumnName("pairAddress");
            entity.Property(e => e.Reserve0)
                .HasMaxLength(90)
                .IsFixedLength();
            entity.Property(e => e.Reserve1)
                .HasMaxLength(90)
                .IsFixedLength();
            entity.Property(e => e.TransactionHash)
                .HasMaxLength(70)
                .IsFixedLength();
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => new { e.TokenAddress, e.ChainId });

            entity.ToTable("Token");

            entity.HasIndex(e => e.LowestReserve, "LowestReserve_Token");

            entity.Property(e => e.TokenAddress)
                .HasMaxLength(42)
                .IsFixedLength();
            entity.Property(e => e.ChainId).HasColumnName("ChainID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.LastBalanceUpdate).HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.LowestReserve)
                .HasMaxLength(90)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(300);
            entity.Property(e => e.NativeTokenBalance).HasMaxLength(80);
            entity.Property(e => e.PriceSymbol)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.PriceUsdt)
                .HasColumnType("decimal(18, 10)")
                .HasColumnName("PriceUSDT");
            entity.Property(e => e.Symbol).HasMaxLength(200);
            entity.Property(e => e.TotalSupply).HasMaxLength(80);
            entity.Property(e => e.FuncSig).HasMaxLength(100);
            entity.Property(e => e.AuthCode).HasMaxLength(100);
            entity.HasOne(d => d.Chain).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.ChainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Token_Chain");
        });

        modelBuilder.Entity<TokensAppearTimesInPair>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TokensAppearTimesInPair");

            entity.Property(e => e.ChainId).HasColumnName("ChainID");
            entity.Property(e => e.Token)
                .HasMaxLength(42)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

}
