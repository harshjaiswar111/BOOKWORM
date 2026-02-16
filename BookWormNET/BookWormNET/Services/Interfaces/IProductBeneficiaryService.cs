using BookWormNET.dto;
using BookWormNET.Models;

namespace BookWormNET.Services.Interfaces
{
    public interface IProductBeneficiaryService
    {
        IEnumerable<ProductBeneficiarydto> GetAll();
        ProductBeneficiary GetById(int id);
        ProductBeneficiary Add(ProductBeneficiary productBeneficiary);
        ProductBeneficiary Update(int id, ProductBeneficiary productBeneficiary);
        bool Delete(int id);
    }
}
