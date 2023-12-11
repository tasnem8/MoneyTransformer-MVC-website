using System;
using System.Collections.Generic;

namespace MoneyTransformer.Models;

public partial class VirtualBank
{
    public decimal Id { get; set; }

    public decimal? Cardnumber { get; set; }

    public DateTime? Expireddate { get; set; }

    public decimal? Cvv { get; set; }

    public decimal? Balance { get; set; }

    public string? Owneremanil { get; set; }

    public string? Ownername { get; set; }
}
