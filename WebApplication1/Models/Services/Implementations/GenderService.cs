using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Services.Interfaces;

namespace WebApplication1.Models.Services.Implementations
{
    public class GenderService : IGenderService
    {
        private readonly AppDbContext _db;
        public GenderService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<Gender>> GetAllGenders()
        {
            var genders = await _db.Genders.Include(x=>x.Students).ToListAsync();
            return genders;
        }

        
    }

        
}
