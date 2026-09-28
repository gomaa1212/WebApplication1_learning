using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repository.Interfaces;
using WebApplication1.SharedRepository.Impelementations;

namespace WebApplication1.Repository.Impelementations
{
    public class StudentImagesRepository :GenericRepository<StudentImages>, IStudentImagesRepository
    {
        #region fields 
        public readonly DbSet<StudentImages> _studentImages;
        #endregion

        #region constructor
        public StudentImagesRepository(AppDbContext context) : base(context)
        {
            _studentImages = context.Set<StudentImages>();
        }
        #endregion

        #region methods
        

        
        #endregion
    }
}
