using BookWormNET.Data;
using BookWormNET.dto;
using BookWormNET.Exceptions;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWormNET.Services.Implementation
{
    public class ProductBeneficiariesService: IProductBeneficiaryService
    {
        private readonly BookwormDbContext _context;

        public ProductBeneficiariesService(BookwormDbContext context)
        {
            _context = context;
        }

        //public IEnumerable<ProductBeneficiary> GetAll()
        //{
        //    return _context.ProductBeneficiaries.ToList();
        //}

        //public ProductBeneficiary GetById(int id)
        //{
        //    return _context.ProductBeneficiaries
        //                   .FirstOrDefault(p => p.ProdbenId == id);
        //}




        public IEnumerable<ProductBeneficiarydto> GetAll()
        {
            return
                (from pb in _context.ProductBeneficiaries
                 join b in _context.Beneficiaries
                      on pb.BeneficiaryId equals b.BeneficiaryId
                 join p in _context.Products
                      on pb.ProductId equals p.ProductId
                 join rc in _context.RoyaltyCalculations
                      on pb.RoycalId equals rc.RoycalId
                 select new ProductBeneficiarydto
                 {
                     ProdbenId = pb.ProdbenId,
                     BeneficiaryName = b.BeneficiaryName,
                     ProductName = p.ProductName,
                     RoyaltyReceived = (decimal)pb.RoyaltyReceived,
                     RoyaltyPercent = rc.RoyaltyPercent,
                     TotalRoyalty = rc.TotalRoyalty
                 })
                .ToList();
        }




        public ProductBeneficiary GetById(int id)
        {
            var pb = _context.ProductBeneficiaries
                             .FirstOrDefault(p => p.ProdbenId == id);

            if (pb == null)
                throw new NotFoundException($"ProductBeneficiary not found with id {id}");

            return pb;
        }

        public ProductBeneficiary Add(ProductBeneficiary productBeneficiary)
        {
            _context.ProductBeneficiaries.Add(productBeneficiary);
            _context.SaveChanges();
            return productBeneficiary;
        }

        public ProductBeneficiary Update(int id, ProductBeneficiary productBeneficiary)
        {
            var existing = _context.ProductBeneficiaries
                                   .FirstOrDefault(p => p.ProdbenId == id);

            if (existing == null)
                return null;

            existing.RoyaltyReceived = productBeneficiary.RoyaltyReceived;
            existing.BeneficiaryId = productBeneficiary.BeneficiaryId;
            existing.ProductId = productBeneficiary.ProductId;
            existing.RoycalId = productBeneficiary.RoycalId;

            _context.SaveChanges();
            return existing;
        }

        public bool Delete(int id)
        {
            var pb = _context.ProductBeneficiaries
                             .FirstOrDefault(p => p.ProdbenId == id);

            if (pb == null)
                return false;

            _context.ProductBeneficiaries.Remove(pb);
            _context.SaveChanges();
            return true;
        }
    }
}

