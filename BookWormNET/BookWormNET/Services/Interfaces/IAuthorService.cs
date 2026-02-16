using BookWormNET.Models;

public interface IAuthorService
{
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author> CreateAsync(Author Author);
}