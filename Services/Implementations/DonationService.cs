using GOTG_Ronewa.Data;
using GOTG.Ronewa.Web.Models;
using GOTG.Ronewa.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GOTG.Ronewa.Web.Services.Implementations
{
    public class DonationService : IDonationService
    {
        private readonly ApplicationDbContext _db;
        public DonationService(ApplicationDbContext db) => _db = db;

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
    }
}
