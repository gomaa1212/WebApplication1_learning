

using WebApplication1.Models.Identity;

namespace WebApplication1.Models.Services.Interfaces
{
    public interface IClaimService
    {
        public Task<List<Claim>> GetAllClaimsAsync();
        public Task<string> AddClaimAsync(Claim claim);
        public Task<string> UpdateClaimAsync(Claim claim);
        public Task<string> DeleteClaimAsync(int claimId);
        public Task<Claim> GetClaimByIdAsync(int Id);

    }
}
