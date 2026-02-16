using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class Transaction
{
    public long TransactionId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public TransactionStatus? Status { get; set; }

    public decimal? TotalAmount { get; set; }

    public TransactionType TransactionType { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<TransactionItem> TransactionItems { get; set; } = new List<TransactionItem>();

    public virtual User? User { get; set; }
}
