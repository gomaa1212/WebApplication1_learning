namespace WebApplication1.Models.ViewModel.Identity.Claims
{
    public class GetClaimsListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsUserClaim { get; set; } 
        public bool IsRoleClaim { get; set; }
    }
}
