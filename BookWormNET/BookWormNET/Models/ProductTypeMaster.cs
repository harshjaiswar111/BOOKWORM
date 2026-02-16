using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class ProductTypeMaster
{
    public int TypeId { get; set; }

    public string? TypeDesc { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
