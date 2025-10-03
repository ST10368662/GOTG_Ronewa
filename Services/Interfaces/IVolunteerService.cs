using GOTG.Ronewa.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GOTG.Ronewa.Web.Services.Interfaces
{
    public interface IVolunteerService
    {
        // Renamed to match controller
        Task<IEnumerable<VolunteerProfile>> GetAllVolunteersAsync();

        // Defined to match controller
        Task<VolunteerProfile?> GetVolunteerProfileByIdAsync(int id);

        Task<VolunteerProfile?> GetVolunteerProfileByUserIdAsync(string userId);

        Task<VolunteerProfile> CreateVolunteerProfileAsync(VolunteerProfile model);

        // Corrected type to return a list of tasks
        Task<IEnumerable<VolunteerTask>> GetAllTasksAsync();
        Task<IEnumerable<VolunteerTask>> GetAssignedTasksAsync(string userId);

        // Corrected return type
        Task AssignTaskAsync(int taskId, string userId);
    }
}