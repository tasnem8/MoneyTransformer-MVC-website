using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTransformer.Models;

public partial class Aboutu
{
    public decimal Id { get; set; }

    public string? Text1 { get; set; }

    public string? Text2 { get; set; }

    public string? Text3 { get; set; }

    public string? Text4 { get; set; }

    public string? Image1path { get; set; }

    public string? Image2path { get; set; }

    public string? Image3path { get; set; }
    [NotMapped]
    public virtual IFormFile? ImageFile1 { get; set; }
    [NotMapped]
    public virtual IFormFile? ImageFile2 { get; set; }
    [NotMapped]
    public virtual IFormFile? ImageFile3 { get; set; }
}
