using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class Attribute
{
    public int AttributeId { get; set; }

    public string? AttributeDesc { get; set; }

    public virtual ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
