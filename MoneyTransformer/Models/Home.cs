using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTransformer.Models;

public partial class Home
{
    public decimal Id { get; set; }

    public string? Text1 { get; set; }

    public string? Text2 { get; set; }

    public string? Text3 { get; set; }

    public string? Image1path { get; set; }

    public string? Image2path { get; set; }

    public string? Image3path { get; set; }

    public string? Logopath { get; set; }

    public string? Email { get; set; }

    public string? Phonenumber { get; set; }

    public string? Image4path { get; set; }

    public string? Facebooklink { get; set; }

    public string? Instagramlink { get; set; }

    [NotMapped]
    public virtual IFormFile? LogoFile { get; set; }

    [NotMapped]
    public virtual IFormFile? ImageFile1 { get; set; }
    [NotMapped]
    public virtual IFormFile? ImageFile2 { get; set; }
}
