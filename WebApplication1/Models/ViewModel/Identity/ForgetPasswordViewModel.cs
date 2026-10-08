using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModel.Identity
{
    public class ForgetPasswordViewModel
    {
        [Required]
        public string EmailOrUserName { get; set; }

    }
}
