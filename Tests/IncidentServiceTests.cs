using GOTG.Ronewa.Web.Models;
using GOTG.Ronewa.Web.Services.Implementations;
using GOTG_Ronewa.Data; // Correct namespace for ApplicationDbContext
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace GOTG.Ronewa.Tests
{
    public class IncidentServiceTests
    {
        // Helper method to create isolated In-Memory DB
        private ApplicationDbContext CreateInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                // Note: Microsoft.EntityFrameworkCore.InMemory NuGet package is required for UseInMemoryDatabase
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        // --- 1. Create Tests (Basic & Edge Case) ---

        [Fact]
        public async Task CreateAsync_AddsIncidentAndReturnsModel()
        {
            using var db = CreateInMemoryDb();
            var service = new IncidentService(db);

            // All required fields (Title, Location, Description, Phone) are set.
            var incident = new IncidentReport
            {
                Title = "Fire",
                Location = "Town C",
                Description = "Structural fire",
                Phone = "011-123-4567"
            };

            var created = await service.CreateAsync(incident);

            Assert.NotNull(created);
            Assert.True(created.Id > 0); // Verify the ID was generated
            Assert.Equal("Fire", created.Title);
            Assert.Single(await db.IncidentReports.ToListAsync());
        }

        // --- 2. Read (GetAll and GetById) Tests ---

        [Fact]
        public async Task GetAllAsync_ReturnsAllAndOrdersByReportedAt()
        {
            using var db = CreateInMemoryDb();
            // Setup two incidents with different times to test the OrderBy logic in the service
            db.IncidentReports.Add(new IncidentReport { Title = "Old", Location = "L1", Description = "Desc A", Phone = "111", ReportedAt = DateTime.Now.AddDays(-1) });
            db.IncidentReports.Add(new IncidentReport { Title = "New", Location = "L2", Description = "Desc B", Phone = "222", ReportedAt = DateTime.Now });
            await db.SaveChangesAsync();

            var svc = new IncidentService(db);
            var all = await svc.GetAllAsync();

            Assert.Equal(2, all.Count);
            // Verify the OrderByDescending(i => i.ReportedAt) logic
            Assert.Equal("New", all.First().Title);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectIncident()
        {
            using var db = CreateInMemoryDb();
            var incident = new IncidentReport { Title = "Specific", Location = "L3", Description = "Desc S", Phone = "333", Id = 99 };
            db.IncidentReports.Add(incident);
            await db.SaveChangesAsync();

            var svc = new IncidentService(db);
            // Use the model's primary key 'Id' (not IncidentReportId)
            var found = await svc.GetByIdAsync(99);

            Assert.NotNull(found);
            Assert.Equal("Specific", found.Title);
            Assert.Equal(99, found.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNullForNonExistentId()
        {
            using var db = CreateInMemoryDb();
            var svc = new IncidentService(db);
            var found = await svc.GetByIdAsync(999);
            Assert.Null(found);
        }

        // --- 3. Update Tests ---

        [Fact]
        public async Task UpdateAsync_ModifiesIncidentData()
        {
            using var db = CreateInMemoryDb();
            var originalIncident = new IncidentReport { Title = "Original", Location = "Old Spot", Description = "Needs update", Phone = "000" };
            db.IncidentReports.Add(originalIncident);
            await db.SaveChangesAsync(); // Save to generate the ID

            var svc = new IncidentService(db);

            // Retrieve and modify
            var incidentToUpdate = await db.IncidentReports.FirstAsync();
            incidentToUpdate.Title = "Updated Title";
            incidentToUpdate.Location = "New Location";

            // Act: Call the UpdateAsync method (which returns void, per your service)
            await svc.UpdateAsync(incidentToUpdate);

            // Assert: Retrieve from DB and check changes
            var updatedIncident = await db.IncidentReports.FindAsync(incidentToUpdate.Id);

            Assert.NotNull(updatedIncident);
            Assert.Equal("Updated Title", updatedIncident.Title);
            Assert.Equal("New Location", updatedIncident.Location);
        }

        // --- 4. Delete Tests ---

        [Fact]
        public async Task DeleteAsync_RemovesIncident()
        {
            using var db = CreateInMemoryDb();
            var incident = new IncidentReport { Title = "To Delete", Location = "L9", Description = "D9", Phone = "999", Id = 100 };
            db.IncidentReports.Add(incident);
            await db.SaveChangesAsync();

            var svc = new IncidentService(db);

            // Act: Call the DeleteAsync method (which returns void, per your service)
            await svc.DeleteAsync(100);

            // Assert: Check if the incident is null after deletion
            var deletedIncident = await db.IncidentReports.FindAsync(100);
            Assert.Null(deletedIncident);
            Assert.Empty(await db.IncidentReports.ToListAsync());
        }

        [Fact]
        public async Task DeleteAsync_HandlesNonExistentIdGracefully()
        {
            using var db = CreateInMemoryDb();
            var svc = new IncidentService(db);

            // Act: Deleting an ID that does not exist should not throw an exception (graceful handling)
            // If the method returns void, the lack of exception is the successful test.
            await svc.DeleteAsync(999);

            Assert.Empty(await db.IncidentReports.ToListAsync());
        }
    }
}