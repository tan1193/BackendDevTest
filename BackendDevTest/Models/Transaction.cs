using System;
using System.Collections.Generic;

namespace BackendDevTest.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? BlockId { get; set; }

    public string Hash { get; set; }

    public string FromAddress { get; set; }

    public string ToAddress { get; set; }

    public decimal? Value { get; set; }

    public decimal? Gas { get; set; }

    public decimal? GasPrice { get; set; }

    public int? TransactionIndex { get; set; }

    public virtual Block Block { get; set; }
}
