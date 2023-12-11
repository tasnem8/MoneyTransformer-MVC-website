using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTransformer.Models;

public partial class Useraccount
{
    public decimal Id { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? ImagePath { get; set; }
    [NotMapped]
    public virtual IFormFile? ImageFile { get; set; }

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
