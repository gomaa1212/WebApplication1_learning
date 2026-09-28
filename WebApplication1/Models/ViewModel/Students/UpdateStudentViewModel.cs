using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.ViewModel.Students
{
    public class UpdateStudentViewModel
    {
        public int Id { get; set; }
        [Remote(
              action: "IsNameEnExistForUpdate",
              controller: "Student",
               HttpMethod = "POST",
               AdditionalFields = nameof(Id),
               ErrorMessage = "This name already exists")]
        public string NameEn { get; set; }
        [Remote(
            action: "IsNameArExistForUpdate",
            controller: "Student",
             HttpMethod = "POST",
             AdditionalFields = nameof(Id),
             ErrorMessage = "الاسم موجود بالفعل")]
        public string NameAr { get; set; }
        public string? FileUrl { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        public int GenderId { get; set; }
        [NotMapped]
        public List<IFormFile>? Files { get; set; }
        public List<string> currentImages { get; set; }= new List<string>();
    }
}
