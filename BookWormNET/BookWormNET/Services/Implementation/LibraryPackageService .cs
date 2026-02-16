using BookWormNET.Data;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using System;

namespace BookWormNET.Services.Implementation
{
    public class LibraryPackageService : ILibraryPackageService
    {
        private readonly BookwormDbContext _context;

        public LibraryPackageService(BookwormDbContext context)
        {
            _context = context;
        }

        public LibraryPackage CreatePackage(LibraryPackage libraryPackage)
        {
            bool exists = _context.LibraryPackages
                .Any(p => p.Name == libraryPackage.Name);

            if (exists)
                throw new Exception("Library package already exists");

            _context.LibraryPackages.Add(libraryPackage);
            _context.SaveChanges();

            return libraryPackage;
        }

        public LibraryPackage GetPackageById(int packageId)
        {
            return _context.LibraryPackages
                .FirstOrDefault(p => p.PackageId == packageId)
                ?? throw new Exception("Library package not found");
        }

        public List<LibraryPackage> GetAllPackages()
        {
            return _context.LibraryPackages.ToList();
        }

        public LibraryPackage GetPackageByName(string name)
        {
            return _context.LibraryPackages
                .FirstOrDefault(p => p.Name == name)
                ?? throw new Exception("Package not found");
        }

        public void DeletePackage(int packageId)
        {
            var pkg = GetPackageById(packageId);
            _context.LibraryPackages.Remove(pkg);
            _context.SaveChanges();
        }
    }
}
