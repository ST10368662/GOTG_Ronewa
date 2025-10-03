using GOTG.Ronewa.Web.Models;

namespace GOTG.Ronewa.Web.Services.Interfaces
{
    public interface IIncidentService
    {
        Task<IncidentReport> CreateAsync(IncidentReport model);
        Task<List<IncidentReport>> GetAllAsync();
        Task<IncidentReport?> GetByIdAsync(int id);
        Task UpdateAsync(IncidentReport model);
    }
}
