using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class Language
{
    public int LanguageId { get; set; }

    public string? LanguageDesc { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
