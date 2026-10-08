using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModel.Identity
{
    public class RegisterViewModel
    {
        [Required]
        public string FullNameEn { get; set; }
        [Required]
        public string FullNameAr { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="The password and confirm password don't match")]
        public string ConfirmPassword { get; set; }
        public string AddressEn { get; set; }
        public string AddressAr { get; set; }
        

    }
}
