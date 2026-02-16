using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int Qty { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
