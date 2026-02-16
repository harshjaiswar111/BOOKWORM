using BookWormNET.Models;

namespace BookWormNET.Services.Interfaces
{
    public interface IGenereService
    {
        List<Genere> GetAll();
        Genere? GetById(int id);
        //Genere Add(Genere genere);
        //Genere? Update(int id, Genere genere);
        //bool Delete(int id);
    }
}