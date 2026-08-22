using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Remote(
            action: "IsNameExist",
            controller: "Student",
             HttpMethod = "POST",
             ErrorMessage = "This name already exists")]
        public string Name { get; set; }
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
