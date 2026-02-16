using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class ProductAttribute
{
    public int ProdAttId { get; set; }

    public string? AtttributeValue { get; set; }

    public int AttributeId { get; set; }

    public int ProductId { get; set; }

    public virtual Attribute Attribute { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
