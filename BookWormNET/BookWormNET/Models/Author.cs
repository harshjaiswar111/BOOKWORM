using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    public string? Bio { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
