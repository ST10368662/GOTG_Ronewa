using GOTG.Ronewa.Web.Models;

namespace GOTG.Ronewa.Web.Services.Interfaces
{
    public interface IDonationService
    {
        Task<Donation> CreateAsync(Donation model);
        Task<List<Donation>> GetAllAsync();
        Task<Donation?> GetByIdAsync(int id);
        Task UpdateAsync(Donation model);
    }
}
