using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Models.ViewModel.Genderss
{
    public class UpdateGenderViewModel 
    {
        public int Id { get; set; }
        [Remote(
              action: "IsNameGenderEnExistForUpdate",
              controller: "Gender",
               HttpMethod = "POST",
               AdditionalFields = nameof(Id),
               ErrorMessage = "This name already exists")]
        public string NameEn { get; set; }
        [Remote(
              action: "IsNameGenderArExistForUpdate",
              controller: "Gender",
               HttpMethod = "POST",
               AdditionalFields = nameof(Id),
               ErrorMessage = "This name already exists")]
        public string NameAr { get; set; }
    }
}
