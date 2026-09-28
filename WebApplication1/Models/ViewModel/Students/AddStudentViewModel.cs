using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.ViewModel.Students
{
    public class AddStudentViewModel
    {

        [Remote(
              action: "IsNameEnExist",
              controller: "Student",
               HttpMethod = "POST",
               ErrorMessage = "This name already exists")]
        public string NameEn { get; set; }
        [Remote(
            action: "IsNameArExist",
            controller: "Student",
             HttpMethod = "POST",
             ErrorMessage = "الاسم موجود بالفعل")]
        public string NameAr { get; set; }
        public string? FileUrl { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        public int GenderId { get; set; }
        
        
        [NotMapped]
        public List<IFormFile>? Files { get; set; }

    }
}
