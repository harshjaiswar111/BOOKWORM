using BookWormNET.dto;
using BookWormNET.Models;

namespace BookWormNET.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> AddAsync(Product product);
        Task<ProductResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductResponseDto>> GetAllAsync();
        Task<Product?> UpdateAsync(int id, Product Updatedproduct);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ProductResponseDto>> GetLibraryProductsAsync();

        List<Product> GetByLanguageDesc(string languageDesc);
        List<Product> GetByGenereDesc(string genereDesc);
        Task<IEnumerable<ProductResponseDto>> SearchByProductNameAsync(string name);
        Task<IEnumerable<ProductResponseDto>> FilterAsync(string? genere, string? language);
    }
}
