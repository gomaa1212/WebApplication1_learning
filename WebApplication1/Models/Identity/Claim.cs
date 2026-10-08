

using WebApplication1.Helper;

namespace WebApplication1.Models.Identity
{
    public class Claim : LocalizableEntity
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public bool IsUserClaim { get; set; } = false;
        public bool IsRoleClaim { get; set; } = false;
    
    }
}
