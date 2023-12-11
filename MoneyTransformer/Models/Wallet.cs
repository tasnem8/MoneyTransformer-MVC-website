using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoneyTransformer.Models;

public partial class Wallet
{
    public decimal Id { get; set; }

    public decimal? Iban { get; set; }

    public decimal? Balance { get; set; }

    public string? Status { get; set; }

    [Display(Name = "For Period")]
    [DisplayFormat(DataFormatString = "{0:MMM-yyyy}")]
    public DateTime? Datecreate { get; set; }

    public decimal? UserId { get; set; }

    public decimal? BankId { get; set; }

    public virtual Bank? Bank { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual Useraccount? User { get; set; }
}
