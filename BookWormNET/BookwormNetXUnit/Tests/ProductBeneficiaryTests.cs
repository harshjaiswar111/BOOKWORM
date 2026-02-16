using BookWormNET.Data;
using BookWormNET.Models;
using BookWormNET.Services.Implementation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookwormNetXunit.Tests
{
    public class ProductBeneficiariesServiceTests
    {
        private BookwormDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<BookwormDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new BookwormDbContext(options);
        }

        // ✅ TEST 1: Add
        [Fact]
        public void Add_ShouldAddProductBeneficiary()
        {
            var context = GetDbContext();
            var service = new ProductBeneficiariesService(context);

            var pb = new ProductBeneficiary
            {
                BeneficiaryId = 1,
                ProductId = 1,
                RoycalId = 1,
                RoyaltyReceived = 1000
            };

            var result = service.Add(pb);

            Assert.NotNull(result);
            Assert.Equal(1, context.ProductBeneficiaries.Count());
        }

        // ✅ TEST 2: GetById
        [Fact]
        public void GetById_ShouldReturnCorrectProductBeneficiary()
        {
            var context = GetDbContext();

            var pb = new ProductBeneficiary
            {
                BeneficiaryId = 1,
                ProductId = 1,
                RoycalId = 1,
                RoyaltyReceived = 1500
            };

            context.ProductBeneficiaries.Add(pb);
            context.SaveChanges();

            var service = new ProductBeneficiariesService(context);

            var result = service.GetById(pb.ProdbenId);

            Assert.NotNull(result);
            Assert.Equal(1500, result.RoyaltyReceived);
        }
    }
}
