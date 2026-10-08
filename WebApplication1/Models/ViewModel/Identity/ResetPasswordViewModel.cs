using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModel.Identity
{
    public class ResetPasswordViewModel
    {
        public string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmNewPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
