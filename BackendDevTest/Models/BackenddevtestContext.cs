using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BackendDevTest.Models;

public partial class BackenddevtestContext : DbContext
{
    public BackenddevtestContext()
    {
    }

    public BackenddevtestContext(DbContextOptions<BackenddevtestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Block> Blocks { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=backenddevtest;uid=root;password=admin@123", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Block>(entity =>
        {
            entity.HasKey(e => e.BlockId).HasName("PRIMARY");

            entity.ToTable("blocks");

            entity.Property(e => e.BlockId).HasColumnName("blockID");
            entity.Property(e => e.BlockNumber).HasColumnName("blockNumber");
            entity.Property(e => e.BlockReward)
                .HasPrecision(50)
                .HasColumnName("blockReward");
            entity.Property(e => e.GasLimit)
                .HasPrecision(50)
                .HasColumnName("gasLimit");
            entity.Property(e => e.GasUsed)
                .HasPrecision(50)
                .HasColumnName("gasUsed");
            entity.Property(e => e.Hash)
                .HasMaxLength(66)
                .HasColumnName("hash");
            entity.Property(e => e.Miner)
                .HasMaxLength(42)
                .HasColumnName("miner");
            entity.Property(e => e.ParentHash)
                .HasMaxLength(66)
                .HasColumnName("parentHash");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PRIMARY");

            entity.ToTable("transactions");

            entity.HasIndex(e => e.BlockId, "blockID");

            entity.Property(e => e.TransactionId).HasColumnName("transactionID");
            entity.Property(e => e.BlockId).HasColumnName("blockID");
            entity.Property(e => e.FromAddress)
                .HasMaxLength(42)
                .HasColumnName("fromAddress");
            entity.Property(e => e.Gas)
                .HasPrecision(50)
                .HasColumnName("gas");
            entity.Property(e => e.GasPrice)
                .HasPrecision(50)
                .HasColumnName("gasPrice");
            entity.Property(e => e.Hash)
                .HasMaxLength(66)
                .HasColumnName("hash");
            entity.Property(e => e.ToAddress)
                .HasMaxLength(42)
                .HasColumnName("toAddress");
            entity.Property(e => e.TransactionIndex).HasColumnName("transactionIndex");
            entity.Property(e => e.Value)
                .HasPrecision(50)
                .HasColumnName("value");

            entity.HasOne(d => d.Block).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.BlockId)
                .HasConstraintName("transactions_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
