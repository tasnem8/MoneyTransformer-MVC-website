using System;
using System.Collections.Generic;

namespace MoneyTransformer.Models;

public partial class Role
{
    public decimal Id { get; set; }

    public string? Rolename { get; set; }

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();
}
