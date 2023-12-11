using System;
using System.Collections.Generic;

namespace MoneyTransformer.Models;

public partial class Userlogin
{
    public decimal Id { get; set; }

    public string? Email { get; set; }

    public string? Passwordd { get; set; }

    public decimal? RoleId { get; set; }

    public decimal? UserId { get; set; }

    public virtual Role? Role { get; set; }

    public virtual Useraccount? User { get; set; }
}
