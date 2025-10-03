using GOTG.Ronewa.Web.Models;
using GOTG.Ronewa.Web.Services.Interfaces;
using GOTG_Ronewa.Data;
using Microsoft.EntityFrameworkCore;

namespace GOTG.Ronewa.Web.Services.Implementations
{
    public class IncidentService : IIncidentService
    {
        private readonly ApplicationDbContext _db;

        public IncidentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IncidentReport> CreateAsync(IncidentReport model)
        {
            _db.IncidentReports.Add(model);
            await _db.SaveChangesAsync();
            return model;
        }

        public async Task<List<IncidentReport>> GetAllAsync()
        {
            return await _db.IncidentReports
                .OrderByDescending(i => i.ReportedAt)
                .ToListAsync();
        }

        public async Task<IncidentReport?> GetByIdAsync(int id)
        {
            return await _db.IncidentReports
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task UpdateAsync(IncidentReport model)
        {
            _db.IncidentReports.Update(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var incident = await _db.IncidentReports.FindAsync(id);
            if (incident != null)
            {
                _db.IncidentReports.Remove(incident);
                await _db.SaveChangesAsync();
            }
        }
    }
}
