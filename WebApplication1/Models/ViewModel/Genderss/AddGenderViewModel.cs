using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Models.ViewModel.Genderss
{
    public class AddGenderViewModel
    {
        [Remote(
              action: "IsNameGenderEnExist",
              controller: "Gender",
               HttpMethod = "POST",
               ErrorMessage = "This name is already exists")]
        public string NameEn { get; set; }
        [Remote(
              action: "IsNameGenderArExist",
              controller: "Gender",
               HttpMethod = "POST",
               ErrorMessage = "This name is already exists")]
        public string NameAr { get; set; }
    }
}
