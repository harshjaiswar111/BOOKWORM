using BookWormNET.Data;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWormNET.Services.Implementation
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly BookwormDbContext _context;

        public BeneficiaryService(BookwormDbContext context)
        {
            _context = context;
        }

        public Beneficiary CreateBeneficiary(Beneficiary beneficiary)
        {
            _context.Beneficiaries.Add(beneficiary);
            _context.SaveChanges();
            return beneficiary;
        }

        public Beneficiary UpdateBeneficiary(int id, Beneficiary beneficiary)
        {
            var existing = _context.Beneficiaries.Find(id);
            if (existing == null) return null!;

            _context.Entry(existing).CurrentValues.SetValues(beneficiary);
            _context.SaveChanges();
            return existing;
        }

        public Beneficiary GetBeneficiaryById(int id)
        {
            return _context.Beneficiaries
                .Include(b => b.Product)
                .FirstOrDefault(b => b.BeneficiaryId == id)!;
        }

        public IEnumerable<Beneficiary> GetAllBeneficiaries()
        {
            return _context.Beneficiaries
                .Include(b => b.Product)
                .ToList();
        }

        public void DeleteBeneficiary(int id)
        {
            var beneficiary = _context.Beneficiaries
                .Include(b => b.ProductBeneficiaries)
                .FirstOrDefault(b => b.BeneficiaryId == id);

            if (beneficiary == null)
                throw new Exception("Beneficiary not found");

            // delete child records first
            _context.ProductBeneficiaries.RemoveRange(beneficiary.ProductBeneficiaries);

            _context.Beneficiaries.Remove(beneficiary);
            _context.SaveChanges();
        }
    }
}
