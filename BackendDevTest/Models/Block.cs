using System;
using System.Collections.Generic;

namespace BackendDevTest.Models;

public partial class Block
{
    public int BlockId { get; set; }

    public int? BlockNumber { get; set; }

    public string Hash { get; set; }

    public string ParentHash { get; set; }

    public string Miner { get; set; }

    public decimal? BlockReward { get; set; }

    public decimal? GasLimit { get; set; }

    public decimal? GasUsed { get; set; }

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();
}
