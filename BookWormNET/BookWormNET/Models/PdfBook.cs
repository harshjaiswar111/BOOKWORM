using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class PdfBook
{
    public int PdfId { get; set; }

    public string? FileName { get; set; }

    public byte[]? PdfData { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
