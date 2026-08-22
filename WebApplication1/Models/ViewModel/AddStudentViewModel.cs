using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.ViewModel
{
    public class AddStudentViewModel
    {
      
      
        public string Name { get; set; }
        public string? FileUrl { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        public int GenderId { get; set; }
        
        
        [NotMapped]
        public List<IFormFile>? Files { get; set; }

    }
}
