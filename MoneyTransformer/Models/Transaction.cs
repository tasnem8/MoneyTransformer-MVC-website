using System;
using System.Collections.Generic;

namespace MoneyTransformer.Models;

public partial class Transaction
{
    public decimal Id { get; set; }

    public DateTime? TranDate { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Commission { get; set; }

    public decimal? SwalletId { get; set; }

    public decimal? RIban { get; set; }

    public virtual Wallet? Swallet { get; set; }
}
