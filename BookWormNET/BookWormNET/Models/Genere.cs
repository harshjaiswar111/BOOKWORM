using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class Genere
{
    public int GenereId { get; set; }

    public string? GenereDesc { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
