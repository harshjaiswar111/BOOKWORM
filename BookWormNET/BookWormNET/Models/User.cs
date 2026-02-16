using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class User
{
    public int UserId { get; set; }

    public ulong? IsAdmin { get; set; }

    public DateOnly? JoinDate { get; set; }

    public string? UserAddress { get; set; }

    public string UserEmail { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string? UserPhone { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<MyLibrary> MyLibraries { get; set; } = new List<MyLibrary>();

    public virtual ICollection<MyShelf> MyShelves { get; set; } = new List<MyShelf>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
