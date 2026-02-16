using BookWormNET.Data;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;

namespace BookWormNET.Services.Implementation
{
    public class LanguageService : ILanguageService
    {
        private readonly BookwormDbContext _context;

        public LanguageService(BookwormDbContext context)
        {
            _context = context;
        }
        public List<Language> GetAll()
        {
            return _context.Languages.ToList();
        }

        public Language? GetById(int id)
        {
            return _context.Languages.Find(id);
        }
    }
}