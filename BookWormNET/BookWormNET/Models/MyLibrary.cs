using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class MyLibrary
{
    public int MyLibId { get; set; }

    public int BooksAllowed { get; set; }

    public int BooksTaken { get; set; }

    public DateOnly? EndDate { get; set; }

    public DateOnly? StartDate { get; set; }

    public int PackageId { get; set; }

    public int? ProductId { get; set; }

    public int UserId { get; set; }

    public virtual LibraryPackage Package { get; set; } = null!;

    public virtual Product? Product { get; set; }

    public virtual User User { get; set; } = null!;
}
