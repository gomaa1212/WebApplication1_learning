namespace WebApplication1.Models.ViewModel.Identity.Claims
{
    public class AddClaimViewModel
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public bool IsUserClaim { get; set; } = false;
        public bool IsRoleClaim { get; set; } = false;
    }
}
