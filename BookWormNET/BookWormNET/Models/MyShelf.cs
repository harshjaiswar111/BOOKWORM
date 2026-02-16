using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public class MyShelf
{
    public int ShelfId { get; set; }

    public DateTime? ProductExpiryDate { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
