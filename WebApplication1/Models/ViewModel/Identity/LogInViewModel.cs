using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModel.Identity
{
    public class LogInViewModel
    {
        [Required(ErrorMessage ="Please Enter Your Email or Username")]
        
        public string EmailOrUsername { get; set; }

        [Required(ErrorMessage = "Please Enter Your Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
