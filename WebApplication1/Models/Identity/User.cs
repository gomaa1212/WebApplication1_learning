using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models.Identity
{
    public class User : IdentityUser
    {
        public string FullNameEn { get; set; }
        public string FullNameAr { get; set; }
        public string AddressEn { get; set; }
        public string AddressAr { get; set; }
    }
}
