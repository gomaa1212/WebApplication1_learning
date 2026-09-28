using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repository.Interfaces;
using WebApplication1.SharedRepository.Impelementations;

namespace WebApplication1.Repository.Impelementations
{
    public class StudentRepository :GenericRepository<Student>, IStudentRepository
    {
        #region Fields
        private readonly DbSet<Student> _students;
        #endregion

        #region Constructor
        public StudentRepository(AppDbContext context) : base(context)
        {
            _students = context.Set<Student>();
        }
        #endregion

        #region Methods

        
        #endregion
    }

}
