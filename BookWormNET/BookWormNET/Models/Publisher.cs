using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
