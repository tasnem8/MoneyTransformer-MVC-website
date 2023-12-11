using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTransformer.Models;

public partial class Bank
{
    public decimal Id { get; set; }

    public string? Namee { get; set; }

    public string? Descreption { get; set; }

    public string? Phonenumber { get; set; }

    public string? Imagepath { get; set; }
    [NotMapped]
    public virtual IFormFile? ImageFile1 { get; set; }

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
    
}
