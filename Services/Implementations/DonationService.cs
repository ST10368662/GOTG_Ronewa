using GOTG_Ronewa.Data;
using GOTG.Ronewa.Web.Models;
using GOTG.Ronewa.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GOTG.Ronewa.Web.Services.Implementations
{
    public class DonationService : IDonationService
    {
        private readonly ApplicationDbContext _db;
        public DonationService(ApplicationDbContext db) => _db = db;

        // --- Existing Methods ---

        public async Task<Donation> CreateAsync(Donation model)
        {
            _db.Donations.Add(model);
            await _db.SaveChangesAsync();
            return model;
        }

        public Task<List<Donation>> GetAllAsync()
        {
            return _db.Donations.OrderByDescending(d => d.DonatedAt).ToListAsync();
        }

        public Task<Donation?> GetByIdAsync(int id)
        {
            return _db.Donations.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task UpdateAsync(Donation model)
        {
            _db.Donations.Update(model);
            await _db.SaveChangesAsync();
        }

        // --- Required New Methods for Test Pass/Coverage ---

        // 1. Fixes the missing 'DeleteAsync' definition
        public async Task DeleteAsync(int id)
        {
            var donation = await _db.Donations.FindAsync(id);
            if (donation != null)
            {
                _db.Donations.Remove(donation);
                await _db.SaveChangesAsync();
            }
        }

        // 2. Fixes the missing 'GetTotalResourceQuantityAsync' definition 
        //    (Crucial for high test coverage business logic)
        public async Task<int> GetTotalResourceQuantityAsync(string resourceType)
        {
            return await _db.Donations
                .Where(d => d.ResourceType == resourceType)
                .SumAsync(d => d.Quantity);
        }
    }
}