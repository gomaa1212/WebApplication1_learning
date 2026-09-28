using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Helper;

namespace WebApplication1.Models
{
    public class Student : LocalizableEntity
    {
        public int Id { get; set; }
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
             ErrorMessage = "This name already exists")]
        public string NameAr { get; set; }
        public string? FileUrl { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        public int GenderId { get; set; }
        public Gender? Gender { get; set; }
        public List<StudentImages>? StudentImages { get; set; } 
        [NotMapped]
        public List<IFormFile>? Files { get; set; }
    }
}
