namespace WebApplication1.Models.ViewModel.Identity.User
{
    public class GetUsersViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<string> RoleName { get; set; }
    }
}
