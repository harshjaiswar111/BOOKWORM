using BookWormNET.Data;
using BookWormNET.Models;
using Microsoft.EntityFrameworkCore;

public class AuthorService : IAuthorService
{
    private readonly BookwormDbContext _context;

    public AuthorService(BookwormDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _context.Authors.ToListAsync();
    }

    public async Task<Author> CreateAsync(Author Author)
    {
        var author = new Author
        {
            Name = Author.Name,
            Bio = Author.Bio
        };

        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return author;
    }
}