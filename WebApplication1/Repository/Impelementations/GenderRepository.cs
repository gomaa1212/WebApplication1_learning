using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repository.Interfaces;
using WebApplication1.SharedRepository.Impelementations;

namespace WebApplication1.Repository.Impelementations
{
    public class GenderRepository :GenericRepository<Gender>, IGenderRepository
    {
        #region fields 
        public readonly DbSet<Gender> _genders;
        #endregion
        #region constructor
        public GenderRepository(AppDbContext context) : base(context)
        {
            _genders = context.Set<Gender>();
        }
        #endregion
        #region methods
        
        #endregion

    }

}
