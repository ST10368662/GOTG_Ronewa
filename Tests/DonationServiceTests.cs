using GOTG.Ronewa.Web.Models;
using GOTG.Ronewa.Web.Services.Implementations;
using GOTG_Ronewa.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace GOTG.Ronewa.Tests
{
    public class DonationServiceTests
    {
        private ApplicationDbContext CreateInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        // --- 1. Create Tests (Basic & Status Logic) ---

        [Fact]
        public async Task CreateAsync_AddsDonationAndSetsDefaultStatus()
        {
            using var db = CreateInMemoryDb();
            var svc = new DonationService(db);

            // Use 'DonatedAt' and 'Quantity', not 'DonationDate' or 'Amount'
            var d = new Donation
            {
                DonorName = "John Doe",
                ResourceType = "Food",
                Quantity = 10,
                DonatedAt = DateTime.Now
            };
            var created = await svc.CreateAsync(d);

            Assert.NotNull(created);
            Assert.True(created.Id > 0);
            Assert.Equal("John Doe", created.DonorName);
            // Verify default business logic is applied
            Assert.Equal(DonationStatus.Pledged, created.Status);
            Assert.Single(await db.Donations.ToListAsync());
        }

        // --- 2. Read Tests (GetAll, GetById, and Order) ---

        [Fact]
        public async Task GetAllAsync_ReturnsAllAndOrdersByDonatedAt()
        {
            using var db = CreateInMemoryDb();
            // Setup data to test the descending order logic
            db.Donations.Add(new Donation { DonorName = "Old", ResourceType = "W", Quantity = 1, DonatedAt = DateTime.Now.AddDays(-2) });
            db.Donations.Add(new Donation { DonorName = "New", ResourceType = "F", Quantity = 1, DonatedAt = DateTime.Now });
            await db.SaveChangesAsync();

            var svc = new DonationService(db);
            var all = await svc.GetAllAsync();

            Assert.Equal(2, all.Count);
            // Verify the OrderByDescending(d => d.DonatedAt) logic
            Assert.Equal("New", all.First().DonorName);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNullForNonExistentId()
        {
            using var db = CreateInMemoryDb();
            var svc = new DonationService(db);
            var found = await svc.GetByIdAsync(999);
            Assert.Null(found);
        }

        // --- 3. Business Logic Tests (Using the required service method) ---

        [Fact]
        public async Task GetTotalResourceQuantityAsync_CalculatesCorrectSumForType()
        {
            using var db = CreateInMemoryDb();
            // Setup: 10 units of Water, 5 units of Food
            db.Donations.Add(new Donation { ResourceType = "Water", Quantity = 5, DonatedAt = DateTime.Now });
            db.Donations.Add(new Donation { ResourceType = "Water", Quantity = 5, DonatedAt = DateTime.Now });
            db.Donations.Add(new Donation { ResourceType = "Food", Quantity = 10, DonatedAt = DateTime.Now });
            await db.SaveChangesAsync();

            var svc = new DonationService(db);
            // Test for the specific resource type
            var totalWater = await svc.GetTotalResourceQuantityAsync("Water");

            Assert.Equal(10, totalWater);
        }

        // --- 4. Update and Delete Tests (Full CRUD Coverage) ---

        [Fact]
        public async Task UpdateAsync_ModifiesDonationDataAndStatus()
        {
            using var db = CreateInMemoryDb();
            var original = new Donation { DonorName = "Old Name", ResourceType = "R", Quantity = 5, DonatedAt = DateTime.Now };
            db.Donations.Add(original);
            await db.SaveChangesAsync();

            var svc = new DonationService(db);
            var donationToUpdate = await db.Donations.FirstAsync();
            donationToUpdate.Quantity = 15;
            donationToUpdate.Status = DonationStatus.Collected; // Test enum change

            await svc.UpdateAsync(donationToUpdate);

            var updated = await db.Donations.FindAsync(donationToUpdate.Id);

            Assert.Equal(15, updated.Quantity);
            Assert.Equal(DonationStatus.Collected, updated.Status);
        }

        [Fact]
        public async Task DeleteAsync_RemovesDonation()
        {
            using var db = CreateInMemoryDb();
            var d = new Donation { DonorName = "To Delete", ResourceType = "X", Quantity = 1, Id = 100, DonatedAt = DateTime.Now };
            db.Donations.Add(d);
            await db.SaveChangesAsync();

            var svc = new DonationService(db);
            // DeleteAsync now exists in the service
            await svc.DeleteAsync(100);

            // Assert
            Assert.Null(await db.Donations.FindAsync(100));
        }
    }
}