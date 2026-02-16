using BookWormNET.Models;

namespace BookWormNET.Services.Interfaces
{
    public interface IBeneficiaryService
    {
        Beneficiary CreateBeneficiary(Beneficiary beneficiary);
        Beneficiary UpdateBeneficiary(int id, Beneficiary beneficiary);
        Beneficiary GetBeneficiaryById(int id);
        IEnumerable<Beneficiary> GetAllBeneficiaries();
        void DeleteBeneficiary(int id);
    }
}
