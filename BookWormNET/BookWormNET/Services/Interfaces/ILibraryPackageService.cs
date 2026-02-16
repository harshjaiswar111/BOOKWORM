using BookWormNET.Models;

namespace BookWormNET.Services.Interfaces
{
    public interface ILibraryPackageService
    {
        LibraryPackage CreatePackage(LibraryPackage libraryPackage);
        LibraryPackage GetPackageById(int packageId);
        List<LibraryPackage> GetAllPackages();
        LibraryPackage GetPackageByName(string name);
        void DeletePackage(int packageId);
    }
}
