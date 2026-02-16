using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class Beneficiary
{
    public int BeneficiaryId { get; set; }

    public string? BeneficiaryAccNo { get; set; }

    public string? BeneficiaryAccType { get; set; }

    public string? BeneficiaryBankBranch { get; set; }

    public string? BeneficiaryBankName { get; set; }

    public string? BeneficiaryContactNo { get; set; }

    public string? BeneficiaryEmailId { get; set; }

    public string? BeneficiaryIfsc { get; set; }

    public string? BeneficiaryName { get; set; }

    public string? BeneficiaryPan { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ProductBeneficiary> ProductBeneficiaries { get; set; } = new List<ProductBeneficiary>();
}
