using System;
using System.Collections.Generic;

namespace MoneyTransformer.Models;

public partial class Contactu
{
    public decimal Id { get; set; }

    public string? Message { get; set; }

    public string? Subject { get; set; }

    public string? Email { get; set; }

    public string? Phonenumber { get; set; }

    public string? Name { get; set; }
}
