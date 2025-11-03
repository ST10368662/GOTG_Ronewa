using GOTG.Ronewa.Web.Models;
using GOTG.Ronewa.Web.Services.Interfaces;
using GOTG_Ronewa.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOTG.Ronewa.Web.Services.Implementations
{
    public class VolunteerService : IVolunteerService
    {
        private readonly ApplicationDbContext _context;

        public VolunteerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VolunteerProfile>> GetAllVolunteersAsync()
        {
            return await _context.VolunteerProfiles.Include(p => p.User).ToListAsync();
        }

        public async Task<VolunteerProfile?> GetVolunteerProfileByIdAsync(int id)
        {
            return await _context.VolunteerProfiles.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<VolunteerProfile?> GetVolunteerProfileByUserIdAsync(string userId)
        {
            return await _context.VolunteerProfiles.Include(p => p.User).FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<VolunteerProfile> CreateVolunteerProfileAsync(VolunteerProfile model)
        {
            _context.VolunteerProfiles.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<IEnumerable<VolunteerTask>> GetAllTasksAsync()
        {
            return await _context.VolunteerTasks.ToListAsync();
        }

        public async Task<IEnumerable<VolunteerTask>> GetAssignedTasksAsync(string userId)
        {
            return await _context.VolunteerTasks
                                 .Where(t => t.AssignedToVolunteerId == userId)
                                 .ToListAsync();
        }

        public async Task AssignTaskAsync(int taskId, string userId)
        {
            var task = await _context.VolunteerTasks.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task != null)
            {
                task.AssignedToVolunteerId = userId;
                task.Status = VolunteerTaskStatus.Assigned;
                await _context.SaveChangesAsync();
            }
        }
    }
}