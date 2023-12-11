using System;
using System.Collections.Generic;

namespace MoneyTransformer.Models;

public partial class Testimonial
{
    public decimal Id { get; set; }

    public string? Text { get; set; }

    public string? Status { get; set; }

    public DateTime? TestimonialDate { get; set; }

    public decimal? UserId { get; set; }

    public virtual Useraccount? User { get; set; }
}
