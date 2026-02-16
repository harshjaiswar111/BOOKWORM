using BookWormNET.Models;

namespace BookWormNET.Services.Interfaces
{
    public interface ILanguageService
    {
        List<Language> GetAll();
        Language? GetById(int id);
    }
}