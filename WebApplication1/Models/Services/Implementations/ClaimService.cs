using WebApplication1.Models.Identity;
using WebApplication1.Models.Services.Interfaces;
using WebApplication1.UnitOfWorks;

namespace WebApplication1.Models.Services.Implementations
{
    public class ClaimService : IClaimService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClaimService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> AddClaimAsync(Claim claim)
        {
            if(claim == null)
            {
                return "Failed";
            }
            await _unitOfWork.Repository<Claim>().AddAsync(claim);
            return "Successful";
        }

        public Task<string> AddClaimAsync(System.Security.Claims.Claim claim)
        {
            throw new NotImplementedException();
        }

        public async Task<string> DeleteClaimAsync(int claimId)
        {
            if(claimId <= 0)
            {
                return "Failed";
            }
            var claim = await _unitOfWork.Repository<Claim>().GetByIdAsync(claimId);
            if(claim == null)
            {
                return "Failed";
            }
            await _unitOfWork.Repository<Claim>().DeleteAsync(claim);
            return "Successful";
        }

        public Task<List<Claim>> GetAllClaimsAsync()
        {
            return _unitOfWork.Repository<Claim>().GetAllListAsync();
        }

        public async Task<string> UpdateClaimAsync(Claim claim)
        {
            var oldClaim = await _unitOfWork.Repository<Claim>().GetByIdAsync(claim.Id);   
            if(oldClaim is null)
            {
                return "Failed";
            }
            oldClaim.NameAr = claim.NameAr;
            oldClaim.NameEn = claim.NameEn;
            oldClaim.IsUserClaim = claim.IsUserClaim;
            oldClaim.IsRoleClaim = claim.IsRoleClaim;
            await _unitOfWork.Repository<Claim>().UpdateAsync(oldClaim);
            return "Successful";
        }
        public async Task<Claim> GetClaimByIdAsync(int Id)
        {
            var claim =await _unitOfWork.Repository<Claim>().GetByIdAsync(Id);
            return claim;
        }
           

        
    }
}
