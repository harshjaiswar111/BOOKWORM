using BookWormNET.Data;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;

namespace BookWormNET.Services.Implementation
{
    public class GenereService : IGenereService
    {

        private readonly BookwormDbContext _context;

        public GenereService(BookwormDbContext context)
        {
            _context = context;
        }
        public List<Genere> GetAll()
        {
            return _context.Generes.ToList();
        }

        public Genere? GetById(int id)
        {
            return _context.Generes.Find(id);
        }
    }
}